$('.lock-checkbox').change(function () {
    let uId = $('#uId').val();
    let token = $('#token').val();
    let lockId = this.id;
    let oldLockStatus = !this.checked;
    let accessType = this.checked ? 1 : 0;
    let urlRequest = 'https://localhost:6050/api/locks/' + uId + '/' + lockId + '/' + accessType;
    $.ajax({
        url: urlRequest,
        type: 'PUT',
        beforeSend: function (xhr) {
            xhr.setRequestHeader('Authorization', 'Bearer ' + token);
        },
        success: function (response) {
            iziToast.show({
                title: 'Success',
                message: 'You successfully ' + (accessType == 1 ? 'open' : 'close') + ' the lock :)',
                color: 'green'
            });
        },
        error: function (response) {
            iziToast.show({
                title: 'Error',
                message: response.responseText + ' :(',
                color: 'red',
                position: 'center',
                overlay: true,
                overlayClose: true,
            });
            $('#' + lockId).prop('checked', oldLockStatus);
        }
    });
});
