/**
 * ElWithGuidTemplate
 * attrs obrigatórios: data-template-params, que é uma string separada por vírgula.
 * Ex.: data-target,dataFimGuid,#
 * São atribuidos respectivamente aos parâmetros attr, varName e previx
 *
 * attr - é o atributo que vai receber o guid
 * varName - é opcional, quando passado, armazena o guid em umá variavel dentro de bag para futuramente poder se recuperada pelo nome
 * prefix - é opcional, se passado, concatenado a esquerda do guid ao atribuir ao atributo
 */
$.extend(tabManager.bag, {
    ElWithGuidTemplate: function (element, tm, attr, varName, prefix = "") {
        element.attr(attr, prefix + tm.NewGuid(varName));
    }
});
