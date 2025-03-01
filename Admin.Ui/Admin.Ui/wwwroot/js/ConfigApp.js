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
