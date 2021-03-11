(function ($) { //declaracion de plug-in
    $.fn.UpgradedGrid = function (MethodsOrOptions) { // Methods or Options son los parametros que se van a utilizar
        options = $.extend({}, $.fn.UpgradedGrid.defaultOptions, MethodsOrOptions);
        if (options.selectOptions === undefined) {
            options.selectOptions = [5, 10, 50, 100];
        }
        if (options.actualPage === undefined) {
            options.actualPage = 1;
        }
        if (options.ultimatePage === undefined) {
            options.ultimatePage = 1;
        }
        if (options.literacy === undefined) {
            options.literacy = false;
        }

        if (options.literacyOptions === undefined) {
            options.literacyOptions = {
                orderBy: "IDEmpleado",
                isAsc: true
            }
        }

        //Variables Globales necesarias:
        var columnHeading = "", field = "", row = "", arrows = '';
        var recordedItems = "", itemsShowed = "", Convertedpages = 0, numerationPages = '', Currentpage = 1;
        var classTable = 'table';
        var table, optionlist, optionChain = '', divSelect, divPagination, divBagde;

        var Methods = {
            init: function (element, modelComponents) { //Metodo que se ejecuta desde un principio, para formar la base de la tabla
                if (element.tagName != "DIV") {
                    alert("El elemento debe de ser un DIV");
                }
                if (modelComponents.Selectable && modelComponents.Multiselectable) {
                    alert("La Selección y Multiselección no pueden estar activas al mismo tiempo.");
                }
                if (modelComponents.literacy) { //Validaciones del ordenamiento, si está activo se agregan filtros y columnas dependiendo de las funciones activas para las filas de la tabla
                    if (modelComponents.Multiselectable) {
                        if (modelComponents.Details) {
                            columnHeading = CreateColumnHeading(modelComponents, arrows, columnHeading, element);
                            columnHeading = '<th class="allSelected" style="display:none;"><span class="glyphicon linked-button glyphicon-ok-sign" style="color: green; font-size: 20px;" onclick="SelectingAll(' + element.id + ')"></span></th><th> Detalles </th>' + columnHeading;
                        } else {
                            columnHeading = CreateColumnHeading(modelComponents, arrows, columnHeading, element);
                            columnHeading = '<th class="allSelected" style="display:none;"><span class="glyphicon linked-button glyphicon-ok-sign " style="color: green; font-size: 20px;" onclick="SelectingAll(' + element.id + ')"></span></th>' + columnHeading;
                        }
                    } else  
                    {
                        if (modelComponents.Details) {
                            columnHeading = CreateColumnHeading(modelComponents, arrows, columnHeading, element);
                            columnHeading = "<th> Detalles </th>" + columnHeading;
                        } else {
                            columnHeading = CreateColumnHeading(modelComponents, arrows, columnHeading, element);
                        }
                    }
                } else {
                    for (var i = 0; i < modelComponents.columnName.length; i++) {
                        columnHeading += "<th>" + modelComponents.columnName[i] + "</th>";
                    }
                }

                // Validaciones que permiten agregar clases a la tabla
                if (modelComponents.bootstrap.table.isBordered) {
                    classTable += ' table-bordered';    
                }
                if (modelComponents.bootstrap.table.isBorderless) {
                    classTable += ' table-borderless';
                }
                if (modelComponents.bootstrap.table.isCondensed) {
                    classTable += ' table-condensed';
                }
                if (modelComponents.bootstrap.table.isOnHover) {
                    classTable += ' table-hover';
                }
                if (modelComponents.bootstrap.table.isStriped) {
                    classTable += ' table-striped';
                }
                var responsive = " table-responsive";
                if (modelComponents.bootstrap.table.isResponsive) {
                    $(this).addClass(responsive);
                }

                table = "<table class=' " + (modelComponents.containsBootstrap ? classTable : '') + " '>"
                    + "<thead><tr>"
                    + columnHeading
                    + "</tr></thead>"
                    + "<tbody>"
                    + row
                    + "</tbody>"
                    + "</table>";

                optionlist = modelComponents.selectOptions.length;
                for (var i = 0; i < optionlist; i++) {
                    optionChain += '<option value="' + (modelComponents.selectOptions[i]) + '">' + modelComponents.selectOptions[i] + '</option>';
                }

                //Creación de las opciones del filtro
                divSelect = '<div class="col-md-3" style="text-align:center">'
                    + '<div class="form-group" style="text-align:center">'
                    + '<label>Cantidad de Registros:</label>'
                    + '<select class="custom-select form-control-sm numRecords" style="width:100px;" onchange="ChangingFilter(' + element.id + "," + Currentpage + ')">'
                    + optionChain
                    + '</select></div></div>';

                divPagination = '<div class="col-md-5 align-content-center" style="text-align:center">'
                    + '<div class="align-content-center" style="text-align:center; margin-left: 25%">'
                    + '<ul class="pagination"><li class="page-link first">'
                    + '<a class="glyphicon glyphicon-chevron-left linked-button" style="text-align:center;" onclick="GoToFirstPage(' + element.id + "," + Currentpage + ')"></a></li>'
                    + '<li class="page-item linked-button theFirst"><a class="page-link back" >&laquo;</a></li>'
                    + numerationPages
                    + '<li class="page-item linked-button theSecond"><a class="page-link next" >&raquo;</a></li>'
                    + '<li class="page-link LAST"><a class="glyphicon glyphicon-chevron-right linked-button last" style="text-align:center;"></a></li>'
                    + '</ul></div></div>';

                divBagde = '<div class="col-md-4" style="text-align:center">'
                    + '<div><p><span class="badge badge-primary itemsShowed">' + itemsShowed + '</span> Registros de <span class="badge badge-primary recordedItems">' + recordedItems + '</span></p>'
                    + '</div></div>';

                TableContainer = '<div class="row"><div class="col-md-12 text-center">' + table + '</div></div>';
                OptionsContainer = '<div class="row">' + divSelect + divPagination + divBagde + '</div>';
                FullContainer = TableContainer + OptionsContainer;

                $(element).append(FullContainer);
                $(element).attr("data-model", JSON.stringify(modelComponents)); // este json es el que se inyecta en el metodo loadGrid
                return Methods.loadGrid.apply(element.arguments, [element, Currentpage]);
            },
            update: function () {

            },
            loadGrid: function (element) {
                $(element).find("tbody").empty();
                $(element).find(".target").remove();
               
                var RecordsToShow = $(element).find(".numRecords").val();
                var strNewModel = $(element).attr("data-model");
                var newModel = JSON.parse(strNewModel);
                var totalColumns = newModel.columnName.length;
                var loadingSign = '';

                if (newModel.Selection) {
                    if (newModel.Details) {
                        loadingSign = '<tr><td colspan="' + (totalColumns + 1) + '" style="font-size:20px; color: red;"> Cargando... </td></tr>';
                    } else { loadingSign = '<tr><td colspan="' + totalColumns + '" style="font-size:20px; color: red;"> Cargando... </td></tr>'; }
                } else {
                    if (newModel.MultiSelection) {
                        if (newModel.Details) {
                            loadingSign = '<tr><td colspan="' + (totalColumns + 2) + '" style="font-size:20px; color: red;"> Cargando... </td></tr>';
                        } else { loadingSign = '<tr><td colspan="' + (totalColumns + 1) + '" style="font-size:20px; color: red;"> Cargando... </td></tr>'; }
                    } else {
                        if (newModel.Details) {
                            loadingSign = '<tr><td colspan="' + (totalColumns + 1) + '" style="font-size:20px; color: red;"> Cargando... </td></tr>';
                        } else { loadingSign = '<tr><td colspan="' + totalColumns + '" style="font-size:20px; color: red;"> Cargando... </td></tr>'; }
                    }   
                }

                var cont = 1;
                var selectedOpt = '';
                var multiselectedOpt = '';

                $(element).find("tbody").append(loadingSign);
                var Params = {
                    filter: RecordsToShow,
                    actualPage: newModel.actualPage,
                    isAsc: newModel.literacyOptions.isAsc,
                    orderBy: newModel.literacyOptions.orderBy,
                    literacy: newModel.literacy
                };
                //Para evitar enviar muchos parámetros, mejor enviamos un JSONString
                var JsonParams = JSON.stringify(Params);
                newModel.Model.JsonParams = JsonParams;
               
                if (newModel.datatype == "json") {

                } else if (newModel.datatype == "ajax") {
                    $.ajax({
                        type: newModel.AjaxTypeParams.type,
                        async: false,
                        url: newModel.AjaxTypeParams.URL,
                        dataType: 'json',
                        data: newModel.Model,
                        success: function (data) {
                            //usar .parse para consumirlo
                            $(element).find("tbody").empty();
                            if (data.model.length > 0) {
                                if (newModel.Selectable) {
                                    if (newModel.Details) {
                                        for (var i = 0; i < data.model.length; i++) {
                                            for (var j = 0; j < data.model[i].length; j++) {
                                                field = RowFunctionalities(field, cont, data, i, j, newModel);
                                            }
                                            field = '<td><span class="glyphicon linked-button glyphicon-plus-sign ' + (i + 1) + '-plus" style="display:inline;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span><span class="glyphicon linked-button glyphicon-minus-sign ' + (i + 1) + '-minus" style="display:none;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span></td>' + field;
                                            selectedOpt = ' onclick="Selection(' + element.id + "," + newModel.Selectable + "," + cont + ')"';
                                            row += "<tr " + selectedOpt + ">" + field + "</tr>";
                                            field = "";
                                            cont++;
                                        }
                                    } else {
                                        for (var i = 0; i < data.model.length; i++) {
                                            for (var j = 0; j < data.model[i].length; j++) {
                                                field = RowFunctionalities(field, cont, data, i, j, newModel);
                                            }
                                            selectedOpt = ' onclick="Selection(' + element.id + "," + newModel.Selectable + "," + cont + ')"';
                                            row += "<tr " + selectedOpt + ">" + field + "</tr>";
                                            field = "";
                                            cont++;
                                        }
                                    }
                                } else
                                    if (newModel.Multiselectable) {
                                        if (newModel.Details) {
                                            for (var i = 0; i < data.model.length; i++) {
                                                for (var j = 0; j < data.model[i].length; j++) {
                                                    field = RowFunctionalities(field, cont, data, i, j, newModel);
                                                }
                                                field = '<td class="allSelected" style="display:none;"></td><td><span class="glyphicon linked-button glyphicon-plus-sign ' + (i + 1) + '-plus" style="display:inline;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span><span class="glyphicon linked-button glyphicon-minus-sign ' + (i + 1) + '-minus" style="display:none;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span></td>' + field;
                                                multiselectedOpt = ' onclick="MultiSelection(' + element.id + "," + newModel.Multiselectable + "," + cont + ')"';
                                                row += "<tr " + multiselectedOpt + ">" + field + "</tr>";
                                                field = "";
                                                cont++;
                                            }
                                        } else {
                                            for (var i = 0; i < data.model.length; i++) {
                                                for (var j = 0; j < data.model[i].length; j++) {
                                                    field = RowFunctionalities(field, cont, data, i, j, newModel);
                                                }
                                                field = '<td class="allSelected" style="display:none;"></td>' + field;
                                                multiselectedOpt = ' onclick="MultiSelection(' + element.id + "," + newModel.Multiselectable + "," + cont + ')"';
                                                row += "<tr " + multiselectedOpt + ">" + field + "</tr>";
                                                field = "";
                                                cont++;
                                            }
                                        }
                                    } else {
                                        if (newModel.Details) {
                                            for (var i = 0; i < data.model.length; i++) {
                                                for (var j = 0; j < data.model[i].length; j++) {
                                                    field = RowFunctionalities(field, cont,data,i,j,newModel);
                                                }
                                                field = '<td><span class="glyphicon linked-button glyphicon-plus-sign ' + (i + 1) + '-plus" style="display:inline;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span><span class="glyphicon linked-button glyphicon-minus-sign ' + (i + 1) + '-minus" style="display:none;" onclick="SeeDetails(' + element.id + "," + (i + 1) + "," + "this" + ')"></span></td>' + field;
                                                row += "<tr>" + field + "</tr>";
                                                field = "";
                                                cont++;
                                            }
                                        } else {
                                            for (var i = 0; i < data.model.length; i++) {
                                                for (var j = 0; j < data.model[i].length; j++) {
                                                    field = RowFunctionalities(field, cont, data, i, j, newModel);
                                                }
                                                row += "<tr>" + field + "</tr>";
                                                field = "";
                                            }
                                        }
                                    }
                                recordedItems = data.items;
                                itemsShowed = data.view;
                                Convertedpages = data.totalPages;
                                Currentpage = data.numPage;
                                $(element).find("tbody").append(row);
                                $(element).find(".last").attr("onclick", "GoToLastPage(" + element.id + "," + Convertedpages + ")");
                                $(element).find(".back").attr("onclick", "GoToBackPage(" + element.id + "," + Currentpage + ")");
                                $(element).find(".next").attr("onclick", "GoToNextPage(" + element.id + "," + Currentpage + ")");
                            } else {
                                alert("No se puede visualizar. Intente más tarde.");
                            }
                        },

                        error: function (status) {
                            alert(status);
                        }
                    });
                } else if (newModel.datatype == "file") {

                }

                if (Convertedpages > 5) {
                    for (var i = Currentpage; i <= (Currentpage + 4); i++) {
                        if (i <= Convertedpages) {
                            numerationPages += '<li class="page-item" id="' + i + '"><a class="page-link linked-button target" id="' + i + '" onclick="GoToOtherPage(' + element.id + "," + i + ')">' + i + '</a></li>';
                        }
                    }
                } else {
                    for (var i = Currentpage; i <= Convertedpages; i++) {
                        numerationPages += '<li class="page-item" id="' + i + '"><a class="page-link linked-button target" id="' + i + '" onclick="GoToOtherPage(' + element.id + "," + i + ')">' + i + '</a></li>';
                    }
                }
                $(element).find(".theFirst").after(numerationPages);
                $(element).find(".itemsShowed").empty().append(itemsShowed);
                $(element).find(".recordedItems").empty().append(recordedItems);
                $.each($(".page-item"), function (i, item) {
                    if (Currentpage == i) {
                        $(element).find("#" + i +"").addClass("active");
                    }
                });
                CheckingActivePages(element, Currentpage, Convertedpages);
            }
        };

        return this.each(function () {
            if (Methods[MethodsOrOptions]) {
                return Methods[MethodsOrOptions].apply(this, Array.prototype.slice.call(arguments, 1));
            } else if (typeof MethodsOrOptions === 'object' || !MethodsOrOptions) {
                //default to init
                return Methods.init.apply(this.arguments, [this, options]);
            } else {
                $.error('Method' + MethodsOrOptions + 'Does not exist on jQuery.tooltip');
            }
        });

        //parametros del plugin
        $.fn.UpgradedGrid.defaultOptions = {
            containsBootstrap: true,
            actualPage: 1,
            ultimatePage: 1,
            bootstrap: {
                version: "4",
                table: {
                    isCondensed: false,
                    isBordered: true,
                    isOnHover: false,
                    isBorderless: false,
                    isStriped: false,
                    isResponsive: false,
                    format: ""
                }
            },
            datatype: '',
            Model: {},
            columnName: [],
            selectOptions: [],
            columnModel: [],
            //JsonTypeParams: {},
            AjaxTypeParams:
            {
                URL: '',
                type: ''
            },
            literacy: false,
            Selectable: false,
            Multiselectable: false,
            Details: true,
            optDetails: {
                urlDetails: ""
                }
            //FileTypeParams: {}
        };
    }

}(jQuery));

function CreateColumnHeading(modelComponents, arrows, columnHeading, element) { //Encabezado de la tabla
    for (var i = 0; i < modelComponents.columnName.length; i++) {
        arrows = '<span class="glyphicon glyphicon-arrow-up linked-button ' + modelComponents.columnName[i] + '-true" style="font-size:15px; color:darkslategray;" onclick="Sorting(' + element.id + "," + "'" + modelComponents.columnName[i] + "'" + "," + true + ')"></span><span class="glyphicon glyphicon-arrow-down linked-button ' + modelComponents.columnName[i] + '-false" style="font-size:15px; color:darkslategray;" onclick="Sorting(' + element.id + "," + "'" + modelComponents.columnName[i] + "'" + "," + false + ')"></span>';
        columnHeading += "<th>" + modelComponents.columnName[i] + arrows + "</th>";
    }
    return columnHeading;
}

function ChangingFilter(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = current;
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function GoToFirstPage(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = current;
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}
    
function GoToLastPage(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = current;
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function GoToOtherPage(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = current;
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function GoToNextPage(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = (current + 1);
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function GoToBackPage(option, current) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    arrOption.actualPage = (current - 1);
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function CheckingActivePages(option, current, converted) {
    if (current == 1 && current == converted) {
        $(option).find(".first").hide();
        $(option).find(".theFirst").hide();
        $(option).find(".theSecond").hide();
        $(option).find(".LAST").hide();
    } else if (current == 1) {
        $(option).find(".first").hide();
        $(option).find(".theFirst").hide();
        $(option).find(".theSecond").show();
        $(option).find(".LAST").show();
    } else if (current != 1 && current < converted) {
        $(option).find(".first").show();
        $(option).find(".theFirst").show();
        $(option).find(".theSecond").show();
        $(option).find(".LAST").show();
    } else if (current == converted) {
        $(option).find(".first").show();
        $(option).find(".theFirst").show();
        $(option).find(".theSecond").hide();
        $(option).find(".LAST").hide();
    }
}
function Sorting(option, colName, sortType) {
    $(option).find("span").show();
    $(option).find("span").show();
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption); 
    if (sortType) {
        arrOption.literacyOptions.orderBy = colName;
        arrOption.literacyOptions.isAsc = true;
        $(option).find("." + colName + "-true").hide();
        $(option).find("." + colName + "-false").show();
    } else {
        arrOption.literacyOptions.orderBy = colName;
        arrOption.literacyOptions.isAsc = false;
        $(option).find("." + colName + "-true").show();
        $(option).find("." + colName + "-false").hide();
    }
    $(option).attr("data-model", JSON.stringify(arrOption));
    $(option).UpgradedGrid("loadGrid");
}

function Selection(option, isSelected, row) {
    if (isSelected) {
        if ($(option).find("." + row + "").hasClass("activated")) {
            $(option).find(".activated").removeClass("table-info activated");
        } else {
            $(option).find(".activated").removeClass("table-info activated");
            $(option).find("." + row + "").addClass("table-info activated");
        } 
    }
}

function MultiSelection(option, isMultiselected, row) {
    if (isMultiselected) {
        if ($(option).find("." + row + "").hasClass("activated")) {
            $(option).find("." + row + "").removeClass("table-info activated");

            if ($(option).find(".activated").length == 0) {
                $(option).find(".allSelected").hide();
            } else {
                $(option).find(".allSelected").show();
            }

        } else {
            $(option).find("." + row + "").addClass("table-info activated");
            $(option).find(".allSelected").show();
        }
    }
}

function SelectingAll(option) {
    if ($(option).find(".activated").length > 0 || $(option).find(".activated").length == 0) {
        if ($(option).find(".allSelected").hasClass("activated")) {
            $(option).find(".activated").removeClass("table-info activated");
            $(option).find(".glyphicon-ok-sign").css("color", "green");
            $(option).find(".allSelected").hide();
        } else {
            $(option).find("td").addClass("table-info activated");
            $(option).find(".glyphicon-ok-sign").css("color", "gray");
        }
    }
}

function SeeDetails(option, row, element) {
    var strOption = $(option).attr("data-model");
    var arrOption = JSON.parse(strOption);
    var appendRow = '<tr><td class="itemDetails-' + row + '" colspan="' + $(option).find("th").length + '"></td></tr>';
    if ($(option).find("." + row + "-plus").hasClass("clicked")) { 
        $(option).find("." + row + "-plus").show();
        $(option).find("." + row + "-plus").removeClass("clicked"); 
        $(option).find("." + row + "-minus").hide();
        $(element).parent().parent().next().remove();
    } else {
        $(option).find("." + row + "-plus").hide();
        $(option).find("." + row + "-plus").addClass("clicked");
        $(option).find("." + row + "-minus").show();
        $(element).parent().parent().after(appendRow);
        $(".itemDetails-" + row + "").load(arrOption.optDetails.urlDetails, { row: row }, function () { });
    }
}

function RowFunctionalities(field, cont, data, i, j, newModel) {
    var opt = newModel.columnModel[j].DataType;
    switch (opt) {
        case "text":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" maxlength="' + newModel.columnModel[j].ExtraDataType.maxlength + '" minlength="' + newModel.columnModel[j].ExtraDataType.minlength + '" ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + ' /></td>';
            break;
        case "number":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" max="' + newModel.columnModel[j].ExtraDataType.max + '" min="' + newModel.columnModel[j].ExtraDataType.min + '" step="' + newModel.columnModel[j].ExtraDataType.step + '" ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + ' /></td>';
            break;
        case "email":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" pattern="' + newModel.columnModel[j].ExtraDataType.pattern + '" size="' + newModel.columnModel[j].ExtraDataType.size + '" ' + (newModel.columnModel[j].ExtraDataType.multiple ? "multiple" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + ' /></td>';
            break;
        case "search":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" size="' + newModel.columnModel[j].ExtraDataType.size + '" ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + ' /></td>';
            break;
        case "tel":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" pattern="' + newModel.columnModel[j].ExtraDataType.pattern + '" size="' + newModel.columnModel[j].ExtraDataType.size + '" ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + '/></td>';
            break;
        case "url":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" pattern="' + newModel.columnModel[j].ExtraDataType.pattern + '" ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + '/></td>';
            break;
        case "range":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" min="' + newModel.columnModel[j].ExtraDataType.min + '" max="' + newModel.columnModel[j].ExtraDataType.max + '" step="' + newModel.columnModel[j].ExtraDataType.step + '" ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + '/></td>';
            break;
        case "datetime-local":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" min="' + newModel.columnModel[j].ExtraDataType.min + '" max="' + newModel.columnModel[j].ExtraDataType.max + '" ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + '/></td>';
            break;
        case "date":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" min="' + newModel.columnModel[j].ExtraDataType.min + '" max="' + newModel.columnModel[j].ExtraDataType.max + '" ' + (newModel.columnModel[j].ExtraDataType.disabled ? "disabled" : "") + '/></td>';
            break;
        case "month":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '"/></td>';
            break;
        case "time":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '"/></td>';
            break;
        case "week":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '"/></td>';
            break;
        case "color":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '"/></td>';
            break;
        case "button":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" ' + newModel.columnModel[j].ExtraDataType.event + '="ExecuteButton(' + data.model[i][j] + ')" ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + '/></td>';
            break;
        case "checkbox":
            var inputs = '';
            if (newModel.columnModel[j].ExtraDataType.multiple != undefined && newModel.columnModel[j].ExtraDataType.multiple != null && newModel.columnModel[j].ExtraDataType.multiple != "") {
                for (k = 0; k < newModel.columnModel[j].ExtraDataType.multiple.length; k++) {
                    inputs += '<input type="' + newModel.columnModel[j].DataType + '" name="' + data.model[i][j] + '-' + cont + '" value="' + data.model[i][j] + '" selected_value="' + newModel.columnModel[j].ExtraDataType.selected_value + '" ' + (newModel.columnModel[j].ExtraDataType.checked ? "checked" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + ' /> ' + data.model[i][j];
                }
            } else {
                inputs += '<input type="' + newModel.columnModel[j].DataType + '" name="' + data.model[i][j] + '-' + cont + '" value="' + data.model[i][j] + '" selected_value="' + newModel.columnModel[j].ExtraDataType.selected_value + '" ' + (newModel.columnModel[j].ExtraDataType.checked ? "checked" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + ' /> ' + data.model[i][j];
            }
            field += '<td class="' + cont + '">' + inputs + '</td>';
            break;
        case "radio":
            var inputs = '';
            if (newModel.columnModel[j].ExtraDataType.multiple != undefined && newModel.columnModel[j].ExtraDataType.multiple != null && newModel.columnModel[j].ExtraDataType.multiple != "") {
                for (k = 0; k < newModel.columnModel[j].ExtraDataType.multiple.length; k++) {
                    inputs += '<input type="' + newModel.columnModel[j].DataType + '" name="' + data.model[i][j] + '-' + cont + '" value="' + data.model[i][j] + '" selected_value="' + newModel.columnModel[j].ExtraDataType.selected_value + '" ' + (newModel.columnModel[j].ExtraDataType.checked ? "checked" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + '/>' + data.model[i][j];
                }
            } else {
                inputs += '<input type="' + newModel.columnModel[j].DataType + '" name="' + data.model[i][j] + '-' + cont + '" value="' + data.model[i][j] + '" selected_value="' + newModel.columnModel[j].ExtraDataType.selected_value + '" ' + (newModel.columnModel[j].ExtraDataType.checked ? "checked" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + '/>' + data.model[i][j];
            }
            field += '<td class="' + cont + '">' + inputs + '</td>';
            break;
        case "file":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" accept="' + newModel.columnModel[j].ExtraDataType.accept + '" ' + (newModel.columnModel[j].ExtraDataType.multiple ? "multiple" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + '/></td>';
            break
        case "image":
            field += '<td class="' + cont + '"><img src="'+ +'" value="' + data.model[i][j] + '" width="' + +'" height="' + +'"></td>';
            break;
        case "password":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" id="' + data.model[i][j] + '" value="' + data.model[i][j] + '" maxlength="' + newModel.columnModel[j].ExtraDataType.maxlength + '" minlength="' + newModel.columnModel[j].ExtraDataType.minlength + '" ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + '/><span class="linked-button glyphicon glyphicon-eye-open" onmousedown="ShowPass(' + data.model[i][j] + ')" onmouseup="ShowPass(' + data.model[i][j] + ')"></span></td>';
            break;
        case "submit":
            field += '<td class="' + cont + '"><input type="' + newModel.columnModel[j].DataType + '" value="' + data.model[i][j] + '" ' + (newModel.columnModel[j].ExtraDataType.readonly ? "readonly" : "") + ' ' + (newModel.columnModel[j].ExtraDataType.required ? "required" : "") + '/></td>';
            break;
        default:
            field += "<td class=" + cont + ">" + data.model[i][j] + "</td>";
            break;
    }
    return field;
}

function ShowPass(id) { //Mostrar u ocultar contraseñas
    var ShowPass = document.getElementById(""+ id +"");
    if (ShowPass.type === "password") {
        ShowPass.type = "text";
    } else {
        ShowPass.type = "password";
    }
}

function ExecuteButton(textAlert) {// Corroborar que si funciona como botón
    alert(textAlert);
}

function InputChecked(value, newModel) {
    debugger;
    if (newModel != "" && newModel == value) {
        return "checked";
    } else {
        return "";
    }
}

    