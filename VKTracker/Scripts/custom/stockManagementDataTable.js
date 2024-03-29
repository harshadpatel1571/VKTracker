﻿function bindStockManagement() {
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
        //ajax: {
        //    url: "/StockManagement/GetStockManagementList/",
        //    type: "POST",
        //    datatype: "json"
        //},
        ajax: {
            url: "/StockManagement/GetStockManagementList/",
            type: "POST",
            datatype: "json",
            data: function (data) {
                data.stockCodeId = $('#StockCodeId').val();
                data.fabricId = $('#FabricId').val();
                data.itemTypeId = $('#ItemTypeId').val();
                data.availableQuantity = $('#AvailableQuantity').val();
                data.locationId = $('#LocationId').val();
                data.stockNo = $('#StockNo').val();
                data.fromDate = $('#FromDate').val();
                data.toDate = $('#ToDate').val();
            }
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
            { data: "id", name: "Id", "autoWidth": true },
            {
                data: "parcelCode", name: "parcelCode", "autoWidth": true,
                render: function (data, type, row) {
                    console.log(row);
                    return formateParcelCode(row.parcelCode, row.arrivalDate, row.chalanNo);
                }
            },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            { data: "totalQuantity", name: "Total Quantity", "autoWidth": true },
            { data: "actualQuantity", name: "Actual Quantity", "autoWidth": true },
            { data: "logUserName", name: "Log User Name", "autoWidth": true },
            {
                data: "createdOn", name: "Created On", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(new Date(data).toUTCString());
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
                        "                                        <i class=\"ri-edit-2-line\" Title=\"Edit\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteStockManagementRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-primary fs-20 sa-warning\" onclick='bindStockManagementLogGrid(" + row.id + ")' data-bs-toggle='modal' data-bs-target='#stockManagementLogModal'>\n" +
                        "                                        <i class=\"ri-history-line\" Title=\"Log History\"></i>\n" +
                        "                                    </a>\n" +
                        "                                </div>";
                }
            },
        ],
        dom: 'Bfrtip',
        order: [[10, 'desc']],
        buttons: [
            {
                extend: 'pdfHtml5',
                text: 'PDF',
                titleAttr: 'Generate PDF',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                }
            },
            {
                extend: 'excelHtml5',
                text: 'Excel',
                titleAttr: 'Generate Excel',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                }
            },
            {
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
                }
            },
            DeleteButton,
            TransferLocationButton,
            DistributeButton,
            'pageLength'
        ]
    }).buttons().container().appendTo('#stockManagementHeader');
}

$("#btnSearch").click(function () {
    const table = $("#gridStockManagement").DataTable();
    table.ajax.reload(null, false);

});

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
        if (checkboxValues.length != 1) {
            $("#divIsFull").hide();
            $("#divQuantity").hide();
        }
        else {
            $("#divIsFull").show();
            $("#divQuantity").show();

            $.ajax({
                url: "/StockManagement/EditStockManagement/" + checkboxValues[0],
                type: "GET",
                success: function (response) {
                    if (response.status) {
                        //$("#stockManageEditForm #TotalQuantity").val(response.data.totalQuantity);
                        $("#distributeForm #LQuantity").text("(" + response.data.actualQuantity + ")");
                    }
                }
            });
        }
        $('#distributeForm #StockCodeId').text(stockIds);
        //$("#distributeForm #LQuantity").text("(" + "response.data.quantity" + ")");
        $('#distributionModal').modal('show');

        var currentDate = new Date();
        $('#DistributionDate').val(formateDateYMD(currentDate));
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

    var status = false;
    if (checkboxValues.length == 1) {
        var aqtn = $("#distributeForm #LQuantity").text();
        var qtn = parseInt($("#Quantity").val());
        var total = parseInt(aqtn.slice(1, -1)) - qtn;
        status = (total >= 0 && qtn >= 0) ? true : false;
    } else {
        status = true;
    }

    if ($("#distributeForm").valid() && status) {
        var data = {
            DistributionDate: $("#DistributionDate").val(),
            PartyId: $("#PartyId").val(),
            IsFull: $('input[name=IsFull]:checked').val(),
            Quantity: $("#Quantity").val(),
            BillNo: $("#BillNo").val(),
            Note: $("#Note").val(),
            LocationId: $("#LocationId").val()
        };
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
        var msg = status ? "" : "please enter valid quantity!";
        console.log(msg);
        $("#distributeForm #Quantity-error").text(msg);
        //$("#distributeForm #quantity-error").text(msg);
        return false;
    }

});

$('#distributionModal').on('hidden.bs.modal', function () {
    checkboxValues = [];
    $("#distributeForm #Quantity-error").text("");
    $('form#distributeForm').trigger("reset");
});


let rowNewlist = [];
$("#ThanNoNew_0").focusout(function () {

    $('#MainNewParent').empty();
    rowNewlist = [];

    var count = $("#ThanNoNew_0").val();
    const th = $("#ChildTableTH").html();
    const td = $("#ChildTableTD").html();

    if (count > 0) {
        $('#MainNewParent').append(th);
        for (var i = 1; i <= count; i++) {
            $('#MainNewParent').append(td.replace('StockCodeIdNew_0', 'StockCodeIdNew_' + (i))
                .replace('FabricIdNew_0', 'FabricIdNew_' + (i))
                .replace('ItemIdNew_0', 'ItemIdNew_' + (i))
                .replace('LocationIdNew_0', 'LocationIdNew_' + (i))
                .replace('TotalQuantityNew_0', 'TotalQuantityNew_' + (i))
                .replace('ChildTableTDTR_0', 'ChildTableTDTR_' + (i))
            );

            rowNewlist.push(i);
        }
    }
});

function addThanQuantity(value) {
    let id = value.id.split("_")[1];
    let val = value.value;
    if (val > 13 || val == 0 || val == "" || val == undefined) {
        alert("Max 13 and Min 1 Than allow.");
        return false;
    }
    else {
        $('#ChildTableTDTR_' + id + ' .tempTd').remove();
        for (var i = 1; i <= val; i++) {
            $('#ChildTableTDTR_' + id).append("<td class=\"tempTd text-danger\"><input class=\"form-control col-1\" id=\"TotalThan_" + id + "_" + i + "\" maxlength=\"5\" name=\"TotalThan_" + id + "_" + i + "\" required></td>");
        }
    }
}

$('#stockManageNewAddModal').on('hidden.bs.modal', function () {
    $('#MainNewParent').empty();
    $('#ThanNoNew_0').val('');
});

$(document).on('click', '#addStockNewManage', function (e) {
    if ($("#stockManageNewAddForm").valid()) {
        var objModelList = [];
        rowNewlist.forEach(i => {
            var ThanListValues = [];
            let qtty = $("#TotalQuantityNew_" + i).val();
            if (qtty == 0 || qtty == "") {
                alert("Not valid data.");
                rowNewlist = [];
                return false;
            }
            else {
                for (var j = 1; j <= qtty; j++) {
                    ThanListValues.push($("#TotalThan_" + i + "_" + j).val());
                }

                var data = {
                    ParcelId: $("#ParcelIdNew_" + i).val(),
                    StockCodeId: $("#StockCodeIdNew_" + i).val(),
                    FabricId: $("#FabricIdNew_" + i).val(),
                    ItemId: $("#ItemIdNew_" + i).val(),
                    LocationId: $("#LocationIdNew_" + i).val(),
                    TotalQuantity: qtty,
                    ThanList: ThanListValues
                };
                objModelList.push(data);
            }
        });

        if (rowNewlist.length > 0) {

            var objModel = {
                ParcelId: $("#ParcelIdNew_0").val(),
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
                    alert('Invalid Data !');
                },
                complete: function () {
                }
            })
        }
    } else {
        return false;
    }
});