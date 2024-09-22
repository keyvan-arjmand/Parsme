class MyUploadAdapter {
    constructor(loader) {
        this.loader = loader;
    }

    upload() {
        return this.loader.file
            .then(file => new Promise((resolve, reject) => {
                this._initRequest();
                this._initListeners(resolve, reject, file);
                this._sendRequest(file);
            }));
    }

    abort() {
        if (this.xhr) {
            this.xhr.abort();
        }
    }

    _initRequest() {
        const xhr = this.xhr = new XMLHttpRequest();
        xhr.open('POST', '/admin/UploadImage', true);
        xhr.responseType = 'json';
    }

    _initListeners(resolve, reject, file) {
        const xhr = this.xhr;
        const loader = this.loader;
        const genericErrorText = `Couldn't upload file: ${file.name}.`;

        xhr.addEventListener('error', () => reject(genericErrorText));
        xhr.addEventListener('abort', () => reject());
        xhr.addEventListener('load', () => {
            const response = xhr.response;
            if (!response || response.error) {
                return reject(response && response.error ? response.error.message : genericErrorText);
            }
            resolve({
                default: response.url
            });
        });

        if (xhr.upload) {
            xhr.upload.addEventListener('progress', evt => {
                if (evt.lengthComputable) {
                    loader.uploadTotal = evt.total;
                    loader.uploaded = evt.loaded;
                }
            });
        }
    }

    _sendRequest(file) {
        const data = new FormData();
        // Set the key to match ASP.NET's `IFormFile` parameter.
        data.append('upload', file);
        this.xhr.send(data);
    }
}

function MyCustomUploadAdapterPlugin(editor) {
    editor.plugins.get('FileRepository').createUploadAdapter = (loader) => {
        return new MyUploadAdapter(loader);
    };
}

window.addEventListener("load", (e) => {
    ClassicEditor
        .create(document.querySelector('.editor'), {
            language: 'en',
            // Editor configuration.
            //پشتیبانی از تگ های HTML
            htmlSupport: {
                allow: [
                    {
                        name: /.*/,
                        attributes: true,
                        classes: true,
                        styles: true
                    }
                ]
            },
            //edit toolbar
            toolbar: {
                items: [
                    'undo', 'redo',
                    '|', 'heading',
                    '|', 'fontColor', 'fontBackgroundColor',
                    '|', 'bold', 'italic', 'strikethrough', 'subscript', 'superscript', 'code',
                    '|', 'uploadImage', 'imageInsert', 'mediaEmbed',
                    '|', 'link', 'blockQuote', 'codeBlock',
                    '|', 'alignment',
                    '|', 'bulletedList', 'numberedList', 'todoList', 'outdent', 'indent',
                    '|', 'htmlEmbed', 'sourceEditing'
                ],
                shouldNotGroupWhenFull: false
            },
            //edit heading
            heading: {
                options: [
                    { model: 'paragraph', title: 'Paragraph', class: 'ck-heading_paragraph' },
                    { model: 'heading1', view: 'h1', title: 'Heading 1', class: 'ck-heading_heading1' },
                    { model: 'heading2', view: 'h2', title: 'Heading 2', class: 'ck-heading_heading2' },
                    { model: 'heading3', view: 'h3', title: 'Heading 3', class: 'ck-heading_heading3' },
                    { model: 'heading4', view: 'h4', title: 'Heading 4', class: 'ck-heading_heading4' },
                    { model: 'heading5', view: 'h5', title: 'Heading 5', class: 'ck-heading_heading5' },
                    { model: 'heading6', view: 'h6', title: 'Heading 6', class: 'ck-heading_heading6' },
                    { model: 'myHeading', view: { name: 'h2', classes: 'myHeadingCss' }, title: 'هدینگ اختصاصی', class: 'ck-myHeadingCss', converterPriority: 'high' }
                ]
            },
            //edit link
            link: {
                addTargetToExternalLinks: true,
                decorators: [
                    { mode: 'manual', label: 'قابلیت دانلود', attributes: { download: 'download'}, defaultValue: false, },
                ],
            },
            //edit font color
            fontColor: {
                colors: [
                    { color: 'red', label: 'قرمز' },
                    { color: 'green', label: 'سبز' },
                    { color: 'blue', label: 'آبی' },
                    { color: 'pink', label: 'صورتی' },
                    { color: '#f7f7f7', label: 'خاکستری' },
                ]
            },
            //edit background color
            fontBackgroundColor: {
                colors: [
                    { color: 'red', label: 'قرمز' },
                    { color: 'green', label: 'سبز' },
                    { color: 'blue', label: 'آبی' },
                    { color: 'pink', label: 'صورتی' },
                ]
            },
            extraPlugins: [MyCustomUploadAdapterPlugin],

        })
        .then(editor => {
            window.editor = editor;

            // اعمال استایل مستقیم برای اطمینان از نمایش اعداد انگلیسی
            const rootElement = editor.editing.view.document.getRoot();
            rootElement.setStyle('direction', 'ltr');
            rootElement.setStyle('unicode-bidi', 'embed');
        })
        .catch(handleSampleError);
});

function handleSampleError(error) {
    const issueUrl = 'https://github.com/ckeditor/ckeditor5/issues';
    const message = [
        'Oops, something went wrong!',
        `Please, report the following error on ${issueUrl} with the build id "npwdoukyfz5-3bd6as171pns" and the error stack trace:`
    ].join('\n');
    console.error(message);
    console.error(error);
}