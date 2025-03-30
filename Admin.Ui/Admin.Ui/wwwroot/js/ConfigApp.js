function closeModal(modalId) {
    var modal = bootstrap.Modal.getInstance(document.getElementById(modalId));
    if (modal) {
        modal.hide();
    }
}
    
window.showToast = () => {
    var toastEl = document.querySelector(".bs-toast");
    var toast = new bootstrap.Toast(toastEl);
    toast.show();
};

window.showToastDelete = () => {
    var toastEl = document.querySelector(".bs-toast-delete");
    var toast = new bootstrap.Toast(toastEl);
    toast.show();
};

function initializeCKEditor(element, dotNetHelper, initialData) {
    return ClassicEditor
        .create(element, {
            toolbar: [
                'heading', '|',
                'bold', 'italic', 'link', 'bulletedList', 'numberedList', '|',
                'outdent', 'indent', '|',
                'blockQuote', 'insertTable', 'undo', 'redo'
            ],
            language: 'fa'
        })
        .then(editor => {
            if (initialData) {
                editor.setData(initialData);
            }

            editor.model.document.on('change:data', () => {
                const data = editor.getData();
                dotNetHelper.invokeMethodAsync('EditorDataChanged', data);
            });

            return editor;
        })
        .catch(error => {
            console.error(error);
            throw error;
        });
}