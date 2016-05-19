define('config-data', function() {
    'use strict';

    return {
        'root': '.fr-designer',

        'getReportByUUIDFrom': '../FastReport.Export.axd?getReport=',
        'saveReportByUUIDTo': '../FastReport.Export.axd?putReport=',
        'makePreviewByUUID': '../FastReport.Export.axd?makePreview=',
        'cookieName': 'ARRAffinity',

        'locales-path': 'locales/#{locale}.js',
        'templates-path': 'views/#{name}.tmpl.html',
        'images-path': 'images/#{image}',

        'scale-mobile': 0.6,
        'scale': 1,

        'net': 9.45, // 9.45 px === 0.25 cm

        'hotkeyProhibited': false,

        'dasharrays': {
            'DashDot': '9, 2, 2, 2',
            'Dot': '2, 2',
            'Solid': '',
            'Dash': '9, 3',
            'DashDotDot': '9, 2, 2, 2, 2, 2'
        },

        'font-names': [
            'Arial', 'Calibri', 'Cambria', 'Cambria Math', 'Candara', 'Comic Sans MS', 'Consolas', 'Constantia', 'Corbel', 'Courier New',
            'Franklin Gothic', 'Gabriola', 'Georgia', 'Impact', 'Lucida Console', 'Lucida Sans Unicode', 'Microsoft Sans Serif',
            'Palatino Linotype', 'Segoe Print', 'Segoe Script', 'Segoe UI', 'Segoe UI Symbol', 'Sylfaen', 'Symbol', 'Tahoma', 'Times New Roman',
            'Trebuchet MS', 'Verdana', 'Webdings', 'Wingbings',
            'Droid Sans Mono', 'Ubuntu Mono'
        ],

        'brackets': '[,]',

        'dialog-controls-sort-order': [
            'ButtonControl', 'CheckBoxControl', 'LabelControl', 'RadioButtonControl', 'TextBoxControl',
        ],

        'components-sort-order': ['TextObject', 'PictureObject', 'ShapeObject', 'LineObject', 'SubreportObject', 'TableObject', 'MatrixObject', 'BarcodeObject', 'RichObject', 'CheckBoxObject', 'MSChartObject', 'SparklineObject', 'CellularTextObject', 'MapObject', 'LinearGauge', 'SimpleGauge'],
        'bands-sort-order': ['ReportTitleBand', 'ReportSummaryBand', 'PageHeaderBand', 'PageFooterBand', 'ColumnHeaderBand', 'ColumnFooterBand', 'DataHeaderBand', 'DataBand', 'DataFooterBand', 'GroupHeaderBand', 'GroupFooterBand', 'ChildBand', 'OverlayBand'],

        'band-indent-top': 9.448,
        'band-indent-opacity': 0.3,

        'minComponentWidthForResizingElements': 40,
        'minComponentHeightForResizingElements': 40,

        'rectButtonWidth': 15,
        'rectButtonHeight': 15,
        'rectButtonOpacity': 0.6,
        'rectButtonFill': 'rgb(183, 36, 29)',

        'circleButtonWidth': 6,
        'circleButtonHeight': 6,
        'circleButtonRadius': 5,
        'circleButtonWidth-mobile': 12,
        'circleButtonHeight-mobile': 12,
        'circleButtonRadius-mobile': 10,
        'circleButtonFill': 'rgb(183, 36, 29)',
        'circleButtonOpacity': 0.6,

        'resizingBandBlockWidth': 100,
        'resizingBandBlockHeight': 15,

        'polylineStroke': 'rgb(183, 36, 29)',
        'polylineStrokeWidth': '1px',
        'selectedPolylineStrokeWidth': '2.5px',
        'polylineFill': 'none',
        'polylineWidth': 4,

        'lineMovingScope': 30,

        'default-band-separator-color': '#C0C0C0',
        'selected-band-separator-color': '#2B579A',

        // show-button?
        'show-properties-button': true,
        'show-events-button': true,
        'show-rt-button': true,
        'show-data-button': true,

        // show-opened-panel?
        'show-properties': false,
        'show-events': false,
        'show-rt': false,
        'show-data': false,

        'fadeout': 150,

        // home, view, components, bands, page
        'default-tab-menu': 'home',

        // default, small, large
        'show-saving-progress': 'default',

        // default, html5, false,
        'notifications': 'default',
        'notifications-mobile': 'default'
    };
});
