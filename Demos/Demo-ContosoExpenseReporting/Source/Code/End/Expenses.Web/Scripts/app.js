(function ($) {
    $(document).ready(function () {
        function getExpense(row) {
            return {
                id: $.trim($(row).attr('data-expense-id')),
                reportId: $.trim($('input[name="Id"]').val()),
                index: $.trim($('td:first', row).text()),
                date: $.trim($('input:eq(0)', row).val()),
                description: $.trim($('input:eq(1)', row).val()),
                category: $.trim($('input:eq(2)', row).val()),
                merchant: $.trim($('input:eq(3)', row).val()),
                billedAmount: $.trim($('input:eq(4)', row).val()),
                transactionAmount: $.trim($('input:eq(5)', row).val()),
                accountNumber: $.trim($('input:eq(0)', row.next()).val()),
                costCenter: $.trim($('input:eq(1)', row.next()).val()),
                receiptUrl: $.trim($('a.receipt-attachment', row).attr('href'))
            };
        }

        function getExpenseReport() {
            var details = [];

            $('tr.expense').each(function () {
                details.push(getExpense($(this)));
            });

            return {
                id: $.trim($('input[name="Id"]').val()),
                name: $.trim($('input[name="Name"]').val()),
                purpose: $.trim($('input[name="Purpose"]').val()),
                comments: $.trim($('textarea[name="Comments"]').val()),
                userName: $.trim($('input[name="username"]').val()),
                details: details
            };
        }

        function updateIndexCells() {
            var index = 1;

            $('tr.expense').each(function () {
                var indexCell = $('td:first', this);
                indexCell.text(index);
                index++;
            });
        }

        function submitReport(report) {
            var location = '/reports';
            location += (report.id > 0) ? '/' + report.id : '';

            $.ajax({
                url: location,
                type: (report.id > 0) ? "PUT" : "POST",
                cache: false,
                data: JSON.stringify(report),
                contentType: 'application/json; charset=utf-8'
            }).done(function (msg) {
                window.location.href = '/reports/' + msg.reportId;
            }).fail(function (jqXhr, textStatus) {
                alert("Request failed: " + textStatus);
            });
        }

        function updateTotal(blink) {
            var total = 0;

            $("input.transaction-amount").each(function () {
                total += parseFloat($(this).val());
            });

            var span = $("p.total span").text(!isNaN(total) ? total.toFixed(2) : "---");
            if (blink) span.fadeOut().fadeIn();
        }

        function showUploadingProgress() {
            var file = $('input[name="PictureFile"]', '#attachReceiptForm').val();

            if (file === "") {
                alert('You must provide a file to upload a receipt.');
                return false;
            }

            var expenseId = $('#attachReceiptForm #expense-id').val();
            var expenseRow = $('tr[data-expense-id="' + expenseId + '"]"');

            $('a.attach', expenseRow).text('View receipt').hide();
            $('span.upload-progress', expenseRow).show();

            $('div.attach-receipt').dialog('close');

            return true;
        }

        function showViewReceiptLink(response) {
            var expenseId = $('#attachReceiptForm #expense-id').val();
            var expenseRow = $('tr[data-expense-id="' + expenseId + '"]"');

            $('span.upload-progress', expenseRow).hide();
            $('a.attach', expenseRow).text('View receipt')
                                         .attr('href', response.url)
                                         .removeClass('attach')
                                         .addClass('receipt-attachment')
                                         .show();
        }

        $("div.expenses-header a.add-new").click(function () {
            $('tr.no-expense').hide();
            var newRow = $($("#detail-template").html()).clone();
            newRow.attr('data-expense-id', (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1));

            $('table').append(newRow);
            $('input:eq(1)', newRow).focus();

            updateIndexCells();
            $("input", newRow).each(function () {
                $(this).attr('name', $(this).attr('name') + "_" + newRow.attr('data-expense-id'));
            });

            $('.datepicker').datepicker('destroy').datepicker();
            $('a.attach', newRow).toggle($('#attachReceiptForm').length > 0);

            $("#expense-report-form").removeData("validator")
                     .removeData("unobtrusiveValidation");

            $.validator.unobtrusive.parse("#expense-report-form");
        });

        $("input.transaction-amount").live('keyup', function () { updateTotal(false); })
                                      .live('change', function () { updateTotal(true); });

        $("a.save-draft").click(function () {
            if (!$('#expense-report-form').valid()) {
                return false;
            }

            submitReport(getExpenseReport());

            return false;
        });

        $("a.submit").click(function () {
            if (!$('#expense-report-form').valid()) {
                return false;
            }

            $("#dialog-confirm").attr('title', "Submit this report?");
            $("#dialog-confirm p").text("This report will be submitted for approval and you will not be able to perform further modifications. Are you sure?");
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 180,
                width: 520,
                modal: true,
                buttons: {
                    "Submit": function () {
                        var report = getExpenseReport();
                        report.statusid = 2; // pending
                        submitReport(report);
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });

            return false;
        });

        $("a.discard").click(function () {
            var discardLocation = $(this).attr('href');

            if ($('input[name="Id"]').val() === '0') {
                $("#dialog-confirm").attr('title', "Discard this report?");
                $("#dialog-confirm p").text("The report won’t be saved. Are you sure you want to discard it?");
            } else {
                $("#dialog-confirm").attr('title', "Discard this report?");
                $("#dialog-confirm p").text("Any changes will be lost. Are you sure you want to discard them?");
            }

            $("#dialog-confirm").dialog({
                resizable: false,
                height: 180,
                width: 520,
                modal: true,
                buttons: {
                    "Discard": function () {
                        location.href = discardLocation;
                        $(this).dialog("close");
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });

            return false;
        });

        $("td a.delete").live('click', function () {
            var row = $(this).closest('tr');
            row.next().remove();
            row.remove();
            
            if ($('tr.expense').length == 0) {
                $('tr.no-expense').show();
            }

            updateTotal();
            updateIndexCells();
        });

        $("#report-approve-form input").live('click', function () {
            if (!$('#report-approve-form').valid()) {
                return false;
            }

            var title = ($(this).hasClass('approve') ? "Approve" : "Reject") + " this report?";
            var message = "This report will be " + ($(this).hasClass('approve') ? "approved" : "rejected") + " and the operation cannot be undone. Are you sure?";
            $("#report-approve-form #action").val($(this).val());

            $("#dialog-confirm").attr('title', title);
            $("#dialog-confirm p").text(message);
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 180,
                width: 520,
                modal: true,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        $("#report-approve-form").submit();
                    },
                    Cancel: function () {
                        $(this).dialog("destroy");
                    }
                }
            });

            return false;
        });

        $('.datepicker').datepicker('destroy').datepicker();

        $('a.more, a.collapse').live('click', function () {
            $(this).attr('class', $(this).hasClass('more') ? 'collapse' : 'more');
            $(this).closest('tr').next().toggle();
        });

        $("a.attach").live('click', function () {
            $("div.attach-receipt").attr('title', 'Attach Receipt');
            $('input[name="PictureFile"]', '#attachReceiptForm').val('');
            $('#attachReceiptForm #expense-id').val($(this).closest('tr').attr('data-expense-id'));
            $("div.attach-receipt").dialog({
                resizable: false,
                height: 100,
                width: 470,
                modal: true
            });
        });

        $("a.delete-report").click(function () {
            var deleteLocation = $(this).attr('href');
            $("#dialog-confirm").attr('title', "Delete this report?");
            $("#dialog-confirm p").text("Are you sure you want to delete this report?");
            $("#dialog-confirm").dialog({
                resizable: false,
                height: 180,
                width: 520,
                modal: true,
                buttons: {
                    "Delete": function () {
                        $(this).dialog("close");
                        location.href = deleteLocation;
                    },
                    Cancel: function () {
                        $(this).dialog("close");
                    }
                }
            });

            return false;
        });

        $("a.receipt-attachment").live('click', function () {
            var link = $(this);
            $.fancybox({
                'padding': 0,
                'href': link.attr('href'),
                'transitionIn': 'elastic',
                'transitionOut': 'elastic'
            });

            return false;
        });

        $('#attachReceiptForm').ajaxForm({
            dataType: 'json',
            beforeSubmit: showUploadingProgress,
            success: function (response) {
                showViewReceiptLink(response);
            }
        });
    });
})(jQuery)