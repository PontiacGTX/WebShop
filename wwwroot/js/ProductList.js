
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.modal-trigger').forEach(el => el.addEventListener('click', SelectDeleteTrigger))
    document.querySelector('#cancelDelete').addEventListener('click', CancelDeleteEvnt);
    document.querySelector('#confirmDelete').addEventListener('click', ConfirmDelete);
    console.log('hey');
});
let triggerBtnId;
function SelectDeleteTrigger(event) {
    triggerBtnId = null;
    triggerBtnId = event.target.id;
}
function CancelDeleteEvnt(event) {
    triggerBtnId = null;
}
function ConfirmDelete(event) {
    let id = triggerBtnId;
    let pId = id.substring(id.indexOf('_') + 1, id.length);
        console.log(pId);
    $.ajax({
        url: '/Product/DeleteProduct?ProductId='+pId,
        type: 'GET',
        success: location.reload(),

    });
}

