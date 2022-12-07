function bindStockManagement() {
    $('#gridStockManagement').DataTable({
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
            url: "/StockManagement/GetStockManagementList/",
            type: "POST",
            datatype: "json"
        },
        columns: [
            {
                data: "id", name: "Id", "autoWidth": true,
                render: function (data, type, row) {
                    return '<div class="form-check ms-3"> ' +
                        '<input class="form-check-input text-center" name="stock" type="checkbox" value=' + data + '>' +
                        '</div>';
                }
            },
            { data: "parcelCode", name: "Parcel Code", "autoWidth": true },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            { data: "totalQuantity", name: "Total Quantity", "autoWidth": true },
            { data: "logUserName", name: "Log User Name", "autoWidth": true },
            {
                data: "createdOn", name: "Created On", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
            {
                bSortable: false,
                autoWidth: true,
                sDefaultContent: "<div class=\"hstack gap-3 flex-wrap\">\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-success fs-20\">\n" +
                    "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteParcelRecord(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        "                                    <a class=\"link-success fs-20 sa-warning\" onclick='editStockManagementRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-edit-2-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteStockManagementRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindStockManagementLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#stockManagementLogModal'>\n" +
                        "                                        <i class=\"ri-history-line\"></i>\n" +
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
            },
            DeleteButton,
            TransferLocationButton,
            DistributeButton,
            'pageLength'
        ]
    }).buttons().container().appendTo('#stockManagementHeader');
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

$(document).ready(function () {
    const table = $('#gridStockManagement').DataTable();
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


function bindStockManagementLogGrid(id) {
    $('#gridStockManagementLog').DataTable().destroy();
    $('#gridStockManagementLog').DataTable({
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
        ajax: {
            url: "/StockManagement/GetStockManagementLogList/" + id,
            type: "POST",
            datatype: "json"
        },
        columns: [
            { data: "action", name: "Action", "autoWidth": true },
            { data: "parcelCode", name: "Parcel Code", "autoWidth": true },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            { data: "totalQuantity", name: "Total Quantity", "autoWidth": true },
            { data: "logUserName", name: "Log User Name", "autoWidth": true },
            {
                data: "createdOn", name: "CreatedOn", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
        ],
        dom: 'Blfrtip',
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                }
            },
            {
                extend: 'csvHtml5',
                text: 'CSV',
                titleAttr: 'Generate CSV',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                }
            },
            {
                extend: 'copyHtml5',
                text: 'Copy',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8]
                }
            }
        ]
    }).buttons().container().appendTo('#gridStockManagementLog_wrapper .col-md-6:eq(0)');
}

$("#editStockManage").click(function () {
    if ($("#stockManageEditForm").valid()) {
        event.preventDefault();
        var formData = $("#stockManageEditForm").serialize();
        $.ajax({
            url: "/StockManagement/SaveStockManagement/",
            type: "POST",
            data: formData,
            dataType: "json",
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
                        $("#stockManageEditForm #close-modal").click();
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
    }
    else {
        return false;
    }
});

function editStockManagementRecord(id) {
    $.ajax({
        url: "/StockManagement/EditStockManagement/" + id,
        type: "GET",
        success: function (response) {
            if (response.status) {
                console.log(response.data);
                $("#stockManageEditModal").modal('show');
                $("#stockManageEditForm #Id").val(response.data.id);
                $("#stockManageEditForm #ParcelId").val(response.data.parcelId);
                $("#stockManageEditForm #StockCodeId").val(response.data.stockCodeId);
                $("#stockManageEditForm #FabricId").val(response.data.fabricId);
                $("#stockManageEditForm #ItemId").val(response.data.itemId);
                $("#stockManageEditForm #LocationId").val(response.data.locationId);
                $("#stockManageEditForm #TotalQuantity").val(response.data.totalQuantity);
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
        }
    })
}

function deleteStockManagementRecord(id) {
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
            type: "POST",
            url: "/StockManagement/DeleteStockManagement/" + id,
            success: function () {
                t.value && Swal.fire({
                    timer: 1500,
                    title: "Deleted.",
                    text: "Your record has been deleted.",
                    icon: "success",
                    showCancelButton: false,
                    showConfirmButton: false,
                    buttonsStyling: !1
                }).then(function () {
                    const table = $("#gridStockManagement").DataTable();
                    table.ajax.reload(null, false);
                });
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
            }
        });
    });
}

let rowlist = [0];
$("#ThanNo_0").focusout(function () {
    var count = $("#ThanNo_0").val();
    rowlist.forEach(i => {
        $('#MainParent').empty();
    });
    rowlist = [0];
    console.log(rowlist);
    const data = '<div id="mainChild_1">' + $("#mainChild_1").html() + '</div>';
    if (count > 0) {
        for (var i = 1; i <= count; i++) {
            var $el = $('#mainChild_1').clone();
            $('#MainParent').append("<hr /><br>");
            //$('#MainParent').append($el);

            $('#MainParent').append(data.replace("Id_0", "Id_" + (i))
                .replace('ParcelId_0', 'ParcelId_' + (i))
                .replace('StockCodeId_0', 'StockCodeId_' + (i))
                .replace('FabricId_0', 'FabricId_' + (i))
                .replace('ItemId_0', 'ItemId_' + (i))
                .replace('LocationId_0', 'LocationId_' + (i))
                .replace('TotalQuantity_0', 'TotalQuantity_' + (i))
                .replace('ThanNo_0', 'ThanNo_' + (i))
                .replace('Thandiv_0', 'Thandiv_' + (i))
            );
            $('#Thandiv_' + i).remove();
            rowlist.push(i);
        }
    }
});

$(document).on('click', '#addStockManage', function (e) {
    if ($("#stockManageAddForm").valid()) {
        var objModelList = [];
        rowlist.forEach(i => {
            var data = {
                Id: $("#Id_" + i).val(),
                ParcelId: $("#ParcelId_" + i).val(),
                StockCodeId: $("#StockCodeId_" + i).val(),
                FabricId: $("#FabricId_" + i).val(),
                ItemId: $("#ItemId_" + i).val(),
                LocationId: $("#LocationId_" + i).val(),
                TotalQuantity: $("#TotalQuantity_" + i).val()
            };
            objModelList.push(data);
        });

        var objModel = {
            StockManagementList: objModelList
        };

        $.ajax({
            url: "/StockManagement/SaveStockManagementList",
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
                        $("#stockManageAddForm #close-modal").click();
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

$('#stockManageAddModal').on('hidden.bs.modal', function () {

    $("#stockManageAddForm #Id").val("");
    $("#stockManageAddForm #ParcelId-error").text("");
    $('form#stockManageAddForm').trigger("reset");


    rowlist.forEach(i => {
        $('#MainParent').empty();
    });
    rowlist = [0];
});

function DeleteStockList() {
    var checkboxValues = [];

    $('input[name=stock]:checked').map(function () {

        var data = {
            Id: $(this).val(),
        };
        checkboxValues.push(data);

    });

    var objModel = {
        StockManagementList: checkboxValues
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
                url: "/StockManagement/DeleteStockManagementList",
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
                            const table = $("#gridStockManagement").DataTable();
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

    $('input[name=stock]:checked').map(function () {

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
    $('input[name=stock]:checked').map(function () {
              
        checkboxValues.push($(this).val());
        stockIds = stockIds + $(this).val() + ",";
    });
    if (checkboxValues.length > 0) {
        console.log(stockIds);
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
            IsFull: $("#IsFull").val(),
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