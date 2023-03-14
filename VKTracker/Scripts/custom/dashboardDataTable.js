$(function () {
    var months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var monthDate = new Date();
    var mm = new Date(monthDate.getFullYear(), monthDate.getMonth() + 1, 0).getDate();
    $('#FromDate').val(1 + '-' + months[monthDate.getMonth()] + '-' + monthDate.getFullYear());
    $('#ToDate').val(mm + '-' + months[monthDate.getMonth()] + '-' + monthDate.getFullYear());
});

function bindRecentDistribution() {
    $('#gridRecentDistribution').DataTable({
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
            datatype: "json",
            data: function (data) {
                data.fromDate = $('#FromDate').val();
                data.toDate = $('#ToDate').val();
            }
        },
        columns: [
            { data: "customerName", name: "Customer Name", "autoWidth": true },
            { data: "customerAddress", name: "Customer Address", "autoWidth": true },
            { data: "stockManagementId", name: "StockManagement Id", "autoWidth": true },
            { data: "stockCode", name: "Stock Code", "autoWidth": true },
            { data: "fabricName", name: "Fabric Name", "autoWidth": true },
            { data: "locationName", name: "Location Name", "autoWidth": true },
            { data: "itemName", name: "Item Name", "autoWidth": true },
            { data: "actualQuantity", name: "ActualQuantity", "autoWidth": true },
            {
                data: "distributionDate", name: "Distribution Date", "autoWidth": true,
                render: function (data, type, row) {
                    return formateDate(data);
                }
            },
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
                    "                                    <a href=\"javascript:void(0);\" class=\"link-danger fs-20 sa-warning\" onclick='deleteRecord(0)'>\n" +
                    "                                        <i class=\"ri-delete-bin-line\"></i>\n" +
                    "                                    </a>\n" +
                    "                                </div>",
                render: function (data, type, row) {
                    return "<div class=\"hstack gap-3 flex-wrap\">\n" +
                        "                                    <a class=\"link-success fs-20 sa-warning\" onclick='editDistributionRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-edit-2-line\" Title=\"Edit\"></i>\n" +
                        "                                    </a>\n" +
                        "                                    <a class=\"link-danger fs-20 sa-warning\" onclick='deleteDistributionRecord(" + row.id + ")'>\n" +
                        "                                        <i class=\"ri-delete-bin-line\" Title=\"Delete\"></i>\n" +
                        "                                    </a>\n" +
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
                extend: 'print',
                text: 'Print',
                titleAttr: 'Copy to clipboard',
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5]
                }
            },
            'pageLength'
        ]
    }).buttons().container().appendTo('#recentDistributionHeader');
}

function deleteDistributionRecord(id) {
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
            url: "/Distribution/DeleteDistribution/" + id,
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
                    const table = $("#gridRecentDistribution").DataTable();
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

function editDistributionRecord(id) {
    $.ajax({
        url: "/Distribution/EditDistribution/" + id,
        type: "GET",
        success: function (response) {
            console.log(response.data);
            if (response.status) {
                $("#distributeForm #Id").val(response.data.id);
                $("#distributeForm #StockCodeId").text(response.data.stockCodeId);
                $("#distributeForm #DistributionDate").val(formateDateYMD(response.data.distributionDate));
                $("#distributeForm #PartyId").val(response.data.partyId);
                $("#distributeForm #BillNo").val(response.data.billNo);
                $("#distributeForm #IsFull").val(response.data.isFull);
                if (response.data.isFull) {
                    $("#IsFull").attr('checked', 'checked');
                }
                $("#distributeForm #LQuantity").text("(" + response.data.actualQuantity + ")");
                $("#distributeForm #Quantity").val(response.data.quantity);
                $("#distributeForm #Note").val(response.data.note);
                $('#distributionModal').modal('show');
            }
        },
        error: function (response) {
            alert('Error!');
        },
        complete: function () {
        }
    })
}

$("#addDistribute").click(function () {

    var status = false;
    var aqtn = $("#distributeForm #LQuantity").text();
    var qtn = parseInt($("#Quantity").val());
    var total = parseInt(aqtn.slice(1, -1)) - qtn;
    status = (total >= 0 && qtn >= 0) ? true : false;

    if ($("#distributeForm").valid() && status) {

        event.preventDefault();          
        var data = {
            Id: $("#Id").val(),
            StockCodeId: $("#StockCodeId").text(),
            DistributionDate: $("#DistributionDate").val(),
            PartyId: $("#PartyId").val(),
            IsFull: $('input[name=IsFull]:checked').val(),
            Quantity: $("#Quantity").val(),
            BillNo: $("#BillNo").val(),
            Note: $("#Note").val(),
        };

        var objModel = {
            objModel: data,
        };
        //var formData = $("#distributeForm").serialize();
        $.ajax({
            url: "/Distribution/SaveDistribution/",
            type: "POST",
            data: objModel,
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
                        const table = $("#gridRecentDistribution").DataTable();
                        table.ajax.reload(null, false);
                        $("#distributeForm #close-modal").click();
                    });
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
        var msg = status ? "" : "please enter valid quantity!";
        $("#distributeForm #Quantity-error").text(msg);
        return false;
    }
});

$('#distributionModal').on('hidden.bs.modal', function () {
    $("#distributeForm #Quantity-error").text("");
    $('form#distributeForm').trigger("reset");
    $("#IsFull").attr('checked', 'false');
    $("#IsFull").removeAttr('checked', 'checked');
});

$("#btnSearch").click(function () {
    if ($("#frmSearch").valid()) {
        const table = $("#gridRecentDistribution").DataTable();
        table.ajax.reload(null, false);
    }
    else {
        return false;
    }
});