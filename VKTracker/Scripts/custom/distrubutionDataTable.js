function bindDistribution() {
    $('#gridDistribution').DataTable({
        paging: true,
        lengthChange: true,
        searching: true,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, -1], [10, 25, 50, 75, 100, 'All']],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        ajax: {
            url: "/Distribution/GetDistributionList/",
            type: "POST",
            datatype: "json"
        },
        columns: [
            {
                data: "id", name: "Id", "autoWidth": true,
                render: function (data, type, row) {
                    return '<div class="form-check ms-3"> ' +
                        '<input class="form-check-input text-center" name="distribute" type="checkbox" value=' + data + '>' +
                        '</div>';
                }
            },
            { data: "stockManagementId", name: "StockManagement Id", "autoWidth": true },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "totalQuantity", name: "TotalQuantity", "autoWidth": true },
            { data: "actualQuantity", name: "ActualQuantity", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            {
                bSortable: false,
                autoWidth: true,
                sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        //"                                    <a class=\"link-success fs-20 sa-warning\" onclick='editRecourd(" + row.id + ")'>\n" +
                        //"                                        <i class=\"ri-edit-2-line\" Title=\"Edit\"></i>\n" +
                        //"                                    </a>\n" +
                        //"                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(" + row.id + ")'>\n" +
                        //"                                        <i class=\"ri-delete-bin-line\" Title=\"Delete\"></i>\n" +
                        //"                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindDistributionLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#distributionLogModal'>\n" +
                        "                                        <i class=\"ri-history-line\" Title=\"Log History\"></i>\n" +
                        "                                    </a>\n" +
                        "                                </div>";
                }
            },
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0,1,2,3,4,5]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0,1,2,3,4,5]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0,1,2,3,4,5]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0,1,2,3,4,5]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0,1,2,3,4,5]
                }
            },
            //DeleteButton,
            //TransferLocationButton,
            //DistributeButton,
            'pageLength'
        ]
    }).buttons().container().appendTo('#distributionHeader');
}

$(document).ready(function () {
    const table = $('#gridDistribution').DataTable();
    $('.dataTables_filter input')
        .unbind()
        .bind('input', function (e) {
            if (this.value.length >= 3 || e.keyCode === 13) {
                table.search(this.value).draw();
            }
            if (this.value === "") {
                table.search("").draw();
            }
        });
});

function bindDistributionLogGrid(id) {
    $('#gridDistributionLog').DataTable().destroy();
    $('#gridDistributionLog').DataTable({
        paging: false,
        lengthChange: false,
        searching: false,
        ordering: true,
        lengthMenu: [[10, 25, 50, 75, 100, -1], [10, 25, 50, 75, 100, 'All']],
        info: true,
        autoWidth: true,
        responsive: true,
        processing: true,
        serverSide: true,
        filter: true,
        scrollX: true,
        ajax: {
            url: "/Distribution/GetDistributionLogList/" + id,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "action", name: "Action", "autoWidth": true },
            { data: "stockManagementId", name: "StockManagement Id", "autoWidth": true },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "totalQuantity", name: "TotalQuantity", "autoWidth": true },
            { data: "actualQuantity", name: "ActualQuantity", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            { data: "logUserName", name: "Log User Name", "autoWidth": true },
            {
                data: "createdOn", name: "CreatedOn", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            }
        ],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            }
        ]
    }).buttons().container().appendTo('#gridDistributionLog_wrapper .col-md-6:eq(0)');
}


var DeleteButton = {
    text: 'Delete',
    action: function (e, dt, node, config) {
        DeleteStockList();
    }
};
var TransferLocationButton = {
    text: 'Transfer Location',
    action: function (e, dt, node, config) {
        TransferLocation();
    }
};
var DistributeButton = {
    text: 'Distribute',
    action: function (e, dt, node, config) {
        AddDistribution();
    }
};

function DeleteStockList() {
    var checkboxValues = [];

    $('input[name=distribute]:checked').map(function () {

        var data = {
            Id: $(this).val(),
        };
        checkboxValues.push(data);

    });

    var objModel = {
        objModel: checkboxValues
    };

    if (checkboxValues.length > 0) {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: !0,
            confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
            cancelButtonClass: "btn btn-danger w-xs mt-2",
            confirmButtonText: "Yes, delete it!",
            buttonsStyling: !1,
            showCloseButton: !0,
        }).then(function (t) {
            if (!t.isConfirmed) return;
            $.ajax({
                url: "/Distribution/DeleteDistributionList",
                type: "POST",
                data: objModel,
                success: function (response) {
                    if (response.status) {
                        t.value && Swal.fire({
                            timer: 1500,
                            title: "Deleted.",
                            text: "Your record has been deleted.",
                            icon: "success",
                            showCancelButton: false,
                            showConfirmButton: false,
                            buttonsStyling: !1
                        }).then(function () {
                            const table = $("#gridDistribution").DataTable();
                            table.ajax.reload(null, false);
                        });
                    }
                },
                error: function (response) {
                    let message = "This entity is being referred somewhere else.";

                    if (response.status === 404) {
                        message = "Entity can not be found.";
                    }

                    t.value && Swal.fire({
                        title: "Unable to delete.",
                        text: message,
                        icon: "warning",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: !1
                    })
                },
                complete: function () {
                }
            });
        });
    }
    else {
        Swal.fire({
            title: "Unable to delete record.",
            text: "Please select atlist one record",
            icon: "error",
            showConfirmButton: false,
            timer: 1500,
            buttonsStyling: !1
        })
    }

}

var checkboxValues = [];
function TransferLocation() {

    $('input[name=distribute]:checked').map(function () {

        var data = {
            Id: $(this).val(),
        };
        checkboxValues.push(data);

    });
    if (checkboxValues.length > 0) {

        $('#transferLocationModal').modal('show');

    }
    else {
        Swal.fire({
            title: "Unable to Transfer location.",
            text: "Please select atlist one record",
            icon: "error",
            showConfirmButton: false,
            timer: 1500,
            buttonsStyling: !1
        })
    }
}

$("#addTransferLocation").click(function () {
    if ($("#transferLocationModalForm").valid()) {
        event.preventDefault
        var objModel = {
            StockManagementList: checkboxValues
        };
        var location = $("#transferLocationModalForm #LocationId").val();

        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: !0,
            confirmButtonClass: "btn btn-primary w-xs me-2 mt-2",
            cancelButtonClass: "btn btn-danger w-xs mt-2",
            confirmButtonText: "Yes, Transfer it!",
            buttonsStyling: !1,
            showCloseButton: !0,
        }).then(function (t) {
            if (!t.isConfirmed) return;
            $.ajax({
                url: "/StockManagement/TransferLocation",
                type: "POST",
                data: { objModel: objModel, locationId: location },
                success: function (response) {
                    if (response.status) {
                        t.value && Swal.fire({
                            timer: 1500,
                            title: "Transfered.",
                            text: "Your loaction has been transfered.",
                            icon: "success",
                            showCancelButton: false,
                            showConfirmButton: false,
                            buttonsStyling: !1
                        }).then(function () {
                            const table = $("#gridStockManagement").DataTable();
                            table.ajax.reload(null, false);
                            $("#transferLocationModalForm #close-modal").click();
                            checkboxValues = [];
                        });
                    }
                },
                error: function (response) {
                    let message = "This entity is being referred somewhere else.";

                    if (response.status === 404) {
                        message = "Entity can not be found.";
                    }

                    t.value && Swal.fire({
                        title: "Unable to delete.",
                        text: message,
                        icon: "warning",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        buttonsStyling: !1
                    })
                },
                complete: function () {
                }
            });
        });
    }
    else {
        return false;
    }
});

function AddDistribution() {
    var stockIds = '';
    $('input[name=distribute]:checked').map(function () {

        checkboxValues.push($(this).val());
        stockIds = stockIds + $(this).val() + ",";
    });
    if (checkboxValues.length > 0) {
        if (checkboxValues.length != 1) {
            $("#divIsFull").hide();
            $("#divQuantity").hide();
        }
        else {
            $("#divIsFull").show();
            $("#divQuantity").show();
        }
        $('#distributeForm #StockCodeId').text(stockIds);
        $('#distributionModal').modal('show');
    }
    else {
        Swal.fire({
            title: "Unable to Add Distribution.",
            text: "Please select atlist one record",
            icon: "error",
            showConfirmButton: false,
            timer: 1500,
            buttonsStyling: !1
        })
    }

};

$("#addDistribute").click(function () {

    if ($("#distributeForm").valid()) {
        var data = {
            DistributionDate: $("#DistributionDate").val(),
            PartyId: $("#PartyId").val(),
            IsFull: $('input[name=IsFull]:checked').val(),
            Quantity: $("#Quantity").val(),
            BillNo: $("#BillNo").val(),
            Note: $("#Note").val(),
        };
        console.log(checkboxValues);
        var objModel = {
            objModel: data,
            StockIds: checkboxValues
        };

        $.ajax({
            url: "/Distribution/SaveDistributionList",
            type: "POST",
            data: objModel,
            success: function (response) {
                if (response.status) {
                    Swal.fire({
                        timer: 1500,
                        title: "Saved.",
                        text: "Your record has been Saved.",
                        icon: "success",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        showCancelButton: false,
                        showConfirmButton: false,
                        buttonsStyling: !1
                    }).then(function () {
                        const table = $("#gridStockManagement").DataTable();
                        table.ajax.reload(null, false);
                        $("#distributeForm #close-modal").click();
                        checkboxValues = [];
                    });
                }
                else {
                    Swal.fire({
                        timer: 1500,
                        title: "Duplicate.",
                        text: response.msg,
                        icon: "error",
                        confirmButtonClass: "btn btn-primary w-xs mt-2",
                        showCancelButton: false,
                        showConfirmButton: false,
                        buttonsStyling: !1
                    })
                }
            },
            error: function (response) {
                alert('Error!');
            },
            complete: function () {
            }
        })
    } else {
        return false;
    }

});

$('#distributionModal').on('hidden.bs.modal', function () {
    checkboxValues = [];
});