/*
usar o prefixo gfn "Global Functions" no início do nome das funções
*/
/**
 * converte a primeira letra em minúsculo
 * @param name
 * @returns ex. idPessoa
 */
function gfnFisrtLetterLower(name) {
    return name.substr(0, 1).toLowerCase() + name.substr(1);
}
/**
 * retorna o valor numérico de um input
 * @param el
 * @returns se for nulo retorna 0
 */
function gfnGetFloat(el) {
    let valor = el.val() || "0";
    return parseFloat(valor.replace(/[.]/g, "").replace(/[,]/g, "."));
}
/**
 * retorna o valor numérico como uma string formatada
 * @param value
 * @param precision
 * @returns ex. 10,36
 */
function gfnFormatFloat(value, precision) {
    return value.toFixed(precision).replace(/[.]/g, "x").replace(/[,]/g, ".").replace(/[x]/g, ",");
}
/**
 * adiciona ou altera um parâmetro em uma url
 * @param url ex. pessoa/editpessoa?nome=aa
 * @param paramName ex. nome
 * @param paramValue ex. bb
 * @returns ex. pessoa/editpessoa?nome=bb
 */
function gfnUrlMergeParam(url, paramName, paramValue) {
    paramValue = encodeURIComponent(paramValue);
    let pIniQueryString = url.indexOf("?");
    if (pIniQueryString === -1)
        return url + "?" + paramName + "=" + paramValue;
    let pParam = url.indexOf(paramName + "=", pIniQueryString);
    if (pParam === -1)
        return url += "&" + paramName + "=" + paramValue;
    else {
        let pFim = url.indexOf("&", pParam);
        if (pFim === -1)
            return url.substring(0, pParam) + paramName + "=" + paramValue;
        else
            return url.substring(0, pParam) + paramName + "=" + paramValue + url.substring(pFim, url.length);
    }
}
/**
 * retorna uma rota padrão sem o id.
 * @param url ex. pessoa/editpessoa/2
 * @returns ex. pessoa/editpessoa/
 */
function gfnGetDefaultRouteWithoutID(url) {
    let index = 0;
    for (index = url.length; index >= 0; index--) {
        if (url[index] === "/")
            break;
    }
    return url.substr(0, index + 1);
}
