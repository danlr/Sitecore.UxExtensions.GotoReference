scContentEditor.prototype.initGotoTargetLinks = function() {
    var selector = '.scComboboxEdit:not([data-gototarget-processed]), .scCombobox:not([data-gototarget-processed]), .scContentControlImage:not([data-gototarget-processed])';

    var elements = $sc(selector);

    if (elements.length > 0) {
        $sc.each(elements, function (index, el) {
            if (!$sc(el).closest('table').hasClass('scDatePickerComboBox') && !$sc(el).closest('table').hasClass('scTimePickerComboBox')) {
                $sc(el).closest('.scEditorFieldMarkerInputCell')
                    .find('.scEditorFieldLabel')
                    .append('<a href="javascript:void(0);" style="float:right;" onclick="scContent.goToTarget(this)">go to target &dash;&gt; </a>');
            }
        });

        $sc(selector).attr('data-gototarget-processed', 1);
    }

    setTimeout(scContent.initGotoTargetLinks, 1000);
};

scContentEditor.prototype.goToTarget = function (el) {
    var control = $sc(el)
				.closest('.scEditorFieldMarkerInputCell')
				.find('input, select');

    var stringToParse = '';

    // javascript:return scContent.activate(this,event,'sitecore://master/{9DFBB778-512F-4BA7-8293-C01503572D30}?lang=en&ver=1&fld={7F046B0A-3BDA-4DF8-A7B8-4F64BC64D0B0}&ctl=FIELD349574105')
    if (control.hasClass('scComboboxEdit')) {
        stringToParse = control.closest('.scContentControl[onfocus]').attr('onfocus');
    } else if (control.hasClass('scCombobox') || control.hasClass('scContentControlImage')) {
        stringToParse = control.attr('onfocus');
    }

    var start = stringToParse.indexOf('{');
    var itemId = stringToParse.substr(start, 38);

    start = stringToParse.indexOf('&fld={') + 5;
    var fieldId = stringToParse.substr(start, 38);

    $sc.get("/api/sitecore/FieldData/GetReferencedItemData?itemId=" + itemId + "&fieldId=" + fieldId, function (data) {
        if (data !== '') {
            scForm.postRequest('', '', '', 'item:load(id=' + data + ')');
        }
    });
}

$sc(function () {
    setTimeout(scContent.initGotoTargetLinks, 1000);
});