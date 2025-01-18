/**
 * Toolkit JavaScript
 */
if (typeof PDS === 'undefined' || PDS === null) {
    var PDS = {};
}

/*
* Closure Definition
*/
(function (namespace, $) {

    $(document).ready(function () {
        $(document).foundation();

        // Alter Menus for Screen Reader Accessibility
        // NOTE: future version of Foundation could conflict with this code.
        var treeItems = $(".vertical.menu.accordion-menu [role='treeitem']");
        var trees = $(".vertical.menu.accordion-menu[role='tree']");
        trees.attr("role", "menubar");
        treeItems.attr("role", "menuitem");

    });
})(PDS, jQuery);
