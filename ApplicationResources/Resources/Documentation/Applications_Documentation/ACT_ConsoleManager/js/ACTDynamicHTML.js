var ACTDynamicSettings = {};

var LastErrorMessage = {};



function ProcessPageLoadActions() {
    ProcessBasicHTMLSelector(settings["current_page_item"])
}

function ProcessBasicHTMLSelector(ULDomObj, ClassName, PageID) {
    var _ulMenuItems = $("#main_menu")[0].children[0];
    var _atag = undefined;
    var _CurrentMenuString = 'ma' + CurrentPageMenuID.toString();

    for (var x = 0; x < _ulMenuItems.children.length; x++) {
        _atag = _ulMenuItems.children[x].children[0];
        if (_atag.id.toString() == _CurrentMenuString) {
            _atag.classList.add(ClassName);
            return;
        }
    }

    _ulMenuItems.children[0].children[0].classList.add("current_page_item");
}

function LoadDictionaryObjectFromJSONFile(VariableToLoad, JSONFileURLPATH, UID) {
    var jqobj = $.getJSON(JSONFileURLPATH, function (data) { })
        .done(function (jqxhr, textStatus, error) {
            // Process The DAta
        })
        .fail(function (jqxhr, textStatus, error) {
            // Log The Error and Return Error
            LastErrorMessage.add().uid = UID;
            LastErrorMessage= "Unable to Load the File; " + textStatus.toString() + "; " + error.toString();
            VariableToLoad = undefined;
        })
        .always(function (jqxhr, textStatus, error) {
            alert("complete");
        });

}