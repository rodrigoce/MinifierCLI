using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MinifierCLI.Algorithm
{
    public class BasicMinifier
    {
        public string MinifyFile(string file, bool squarify)
        {
            var sr = new StreamReader(file);
            var sb = new StringBuilder();

            var line = sr.ReadLine();

            int charCount = 0;
            while (line != null)
            {
                string minLine = MinLine(line);
                if (_openedStringWith == string.Empty)
                    minLine = minLine.Trim();

                if (!string.IsNullOrEmpty(minLine))
                {
                    if ((charCount < 200) && (squarify) &&
                        (_openedStringWith == string.Empty) && (_sholdBreakLine == false) /*&& 
                        false*/)
                    {
                        sb.Append(minLine + " ");
                        charCount += minLine.Length;
                    }
                    else
                    {
                        sb.AppendLine(minLine);
                        charCount = 0;
                        _sholdBreakLine = false;
                    }
                }
                line = sr.ReadLine();
            }

            return sb.ToString();
        }

        private const string _beginLongComm = "/*";
        private const string _endLongComm = "*/";
        private const string _lineComm = "//";

        private bool _waitingCloseComment = false;

        private string MinLine(string line)
        {

            /* 
            verificar quem foi aberto antes: // ou /*
              se for // tudo a frente na mesma linha está comentado
              se for /* tudo a frente e até o próximo fechamento está comentado
              no entando, se uma string striver aberta quer dizer que os caracteres
               de de comentário não comentam nada
            */

            // não pode estar esperando fechar comentário ou string
            if ((!_waitingCloseComment) && (_openedStringWith == string.Empty))
            {
                FindOpenComm(ref line);
            }
            else if (_openedStringWith != string.Empty)
            {
                // tenta encontrar o fechamento da string
                // depois tentar encontrar abertura de comentário
                int indexClosedString = FindString(line, true);

                if (indexClosedString > -1)
                {
                    string lineAfterString = line.Substring(indexClosedString + 1);
                    FindOpenComm(ref lineAfterString);
                    line = line.Substring(0, indexClosedString + 1) + lineAfterString;
                }
            }
            else
            {
                if (_waitingCloseComment)
                    FindCloseLongComm(ref line);
            }

            return line;

        }

        private bool _sholdBreakLine = false;
        private void FindOpenComm(ref string line)
        {
            // buscar por abridores de comentário
            int indexLineComm = line.IndexOf(_lineComm);
            int indexBeginLongComm = line.IndexOf(_beginLongComm);

            // pegamos o menor
            int indexComm = -1;
            string typeComm = string.Empty;
            if ((indexLineComm > -1) && (indexBeginLongComm > -1))
            {
                if (indexLineComm < indexBeginLongComm)
                {
                    indexComm = indexLineComm;
                    typeComm = _lineComm;
                }
                else
                {
                    indexComm = indexBeginLongComm;
                    typeComm = _beginLongComm;
                }
            }
            else if ((indexLineComm > -1) && (indexBeginLongComm == -1))
            {
                indexComm = indexLineComm;
                typeComm = _lineComm;
            }
            else if ((indexLineComm == -1) && (indexBeginLongComm > -1))
            {
                indexComm = indexBeginLongComm;
                typeComm = _beginLongComm;
            }

            // se encontrou um abridor de comentário,
            // verifica se não há uma string aberta anterior ao comentário
            if (indexComm > -1)
            {
                string testOpenedStringBefore = line.Substring(0, indexComm);
                FindString(testOpenedStringBefore, false);
                if (_openedStringWith != string.Empty)
                {
                    // pega o texto após o comentário
                    string textAfterComm = line.Substring(indexComm + typeComm.Length);
                    // cancela a abertura de comentário
                    indexComm = -1;
                    // pega o indice de onde a string foi fechada
                    int indexClosedString = FindString(textAfterComm, true);

                    if (indexClosedString > -1)
                    {
                        string lineAfterString = textAfterComm.Substring(indexClosedString + 1);
                        string lineWithString = line.Substring(0, line.Length + 1 - (textAfterComm.Length - indexClosedString));
                        FindOpenComm(ref lineAfterString);
                        line = lineWithString + lineAfterString;
                    }
                }
            }

            // se um comentário foi aberto
            if (indexComm > -1)
            {
                // regex estão entre barras / expressão /
                // ex: /abc//
                // no exemplo acima não há comentário
                // por simplicidade, quando um comentário for aberto
                // e houver anterior a ele uma barra, será considerado 
                // como expressão regex.
                // a quebra de linha deve ser vetada

                if (line.Substring(0, indexComm).IndexOf("/") > -1)
                {
                    _sholdBreakLine = true;
                    indexComm = -1;
                }
                else
                {

                    if (typeComm == _lineComm)
                        line = line.Substring(0, indexComm);
                    else
                    {
                        _waitingCloseComment = true;
                        string restRight = line.Substring(indexComm + typeComm.Length);
                        line = line.Substring(0, indexComm);
                        FindCloseLongComm(ref restRight);
                        line = line + restRight;
                    }
                }
            }
        }

        private void FindCloseLongComm(ref string line)
        {
            int indexCloseLongComm = line.IndexOf(_endLongComm);

            // fechou o comentário longo
            if (indexCloseLongComm > -1)
            {
                line = line.Substring(indexCloseLongComm + _endLongComm.Length);
                _waitingCloseComment = false;
                FindOpenComm(ref line);
            }
            else
                line = string.Empty;
        }

        private const string stringPattern = "(\\\\?)?([\"'`])"; // abridores/fechadores de string precedidos ou não de escape
        private string _openedStringWith = string.Empty;

        /// <summary>
        /// alimenta a variável _openedStringWith indicando se há string aberta
        /// </summary>
        /// <param name="line">linha a ser avaliada</param>
        /// <param name="returnWhenCloseString">se true, termina o algoritmo quando a string for fechada
        /// só passe true se há string aberta</param>
        /// <returns>-1, ou o index de quem fechou a string, quando returnWhenCloseString for true</returns>
        private int FindString(string line, bool returnWhenCloseString)
        {

            MatchCollection matchs = Regex.Matches(line, stringPattern);

            foreach (Match match in matchs)
            {
                // se iniciar com um escape não é um abridor ou fechador de string
                if (match.Value.StartsWith("\\"))
                    continue;

                // registra qual caracterer abriu a string
                if (_openedStringWith == string.Empty)
                {
                    _openedStringWith = match.Value;
                    continue;
                }

                // se não for o mesmo caracter que abriu a string então ela continua fechada
                if (_openedStringWith != match.Value)
                    continue;

                // se chegar aqui a string foi fechada.
                _openedStringWith = string.Empty;
                if (returnWhenCloseString)
                    return match.Index;

            }

            return -1;

        }
    }
}