function defaultSummerNote() {
    var def = $(".summernote").val();
    $("#DraftContentEncoded").val(encodeURIComponent(def));
    $(".summernote").summernote({
        height: 500,
        fontSizes: ['8', '9', '10', '11', '12', '13', '14', '16', '18', '24', '36', '48'],
        toolbar: [
            ['style', ['style']],
            ['fontoptions', ['fontname', 'fontsize']],
            ['font', ['bold', 'italic', 'underline', 'strikethrough', 'superscript', 'subscript', 'color', 'clear']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['height', ['height']],
            ['insert', ['link', 'table', 'picture', 'video']],
            ['view', ['fullscreen', 'codeview', 'help']],
            ['options', ['undo', 'redo']]
        ],
        placeholder: "Write your article content here",
        callbacks: {
            onChange: function (contents, $editable) {
                $("#DraftContentEncoded").val(encodeURIComponent(contents));
            },
            //callbacks: {
            //    onImageLinkInsert: function (url) {
            //        // url is the image url from the dialog
            //        $img = $('<img>').attr({ src: url, alt: "The Juice Press" })
            //        $summernote.summernote('insertNode', $img[0]);
            //    }
            //}
            //onImageUpload: function (files) {
            //    console.log(files);
            //}
        },
        popover: {
            image: [
                ['custom', ['imageAttributes']],
                ['image', ['resizeFull', 'resizeHalf', 'resizeQuarter', 'resizeNone']],
                ['float', ['floatLeft', 'floatRight', 'floatNone']],
                ['remove', ['removeMedia']]
            ],
        },
        lang: 'en-US',
        imageAttributes: {
            icon: '<i class="note-icon-pencil"/>',
            removeEmpty: true, // true = remove attributes | false = leave empty if present
            disableUpload: true // true = don't display Upload Options | Display Upload Options
        }
    });
    //$('.summernote').summernote('insertImage', url, function ($image) {
    //    //$image.css('width', $image.width() / 3);
    //    $image.attr('alt', 'The Juice Press');
    //});
}

$(function () {
    
    
    defaultSummerNote();
    //$(".summernote").summernote("code", def );
})