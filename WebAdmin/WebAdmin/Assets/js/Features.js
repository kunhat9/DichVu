"use strict"
class Features {


    constructor(url_get, dataModel, getParamsForSearch) {
        this.url_Get = url_get;
        this.index;
        this.dataModel = dataModel;
        this.update = false;
        this.Pagination(this, getParamsForSearch);
        this.Search(this, getParamsForSearch);
        this.changePageNumber(this, getParamsForSearch);
        this.key = {
            "VOUCHER_CODE": "mã khuyến mãi",
            "VOUCHER_NAME": "tên khuyến mãi",
            "PACKAGE_NAME": "tên gói tập",
            "PACKAGE_BOX_ID": "chi nhánh",
            "PRODUCT_CODE": "mã sản phẩm",
            "PRODUCT_NAME": "tên sản phẩm",
            "PRODUCT_PRICE": "giá sản phẩm",
            "TYPE_NAME": "tên loại danh mục",
            "ACTION_PARENT_ID": "mã action cha",
            "ACTION_NAME": "tên action",
            "ACTION_CONTROLPATH": "đường dẫn action",
            "VOUCHER_TYPE": "kiểu khuyến mãi",
            "PACKAGE_PRICE": "giá gói tập",
            "PACKAGE_TYPE": "loại gọi tập",
            "BOX_ADDRESS": "địa chỉ",
            "USER_FULLNAME": "họ tên"
        }
    }
    // Set event
    Pagination(mySelf, getParamsForSearch) {
        $(".page").unbind();
        $(".page").click(function () {
            let pageNumber = parseInt($(this).find(".page-link").text());
            mySelf.ajax_getTableData(getParamsForSearch(pageNumber, 0));
        })
        $(".prev").unbind();
        $(".prev").not(".disabled").click(function () {
            let pageNumber = parseInt($(".active>.page-link").text());
            mySelf.ajax_getTableData(getParamsForSearch(pageNumber, -1));
        })
        $(".next").unbind();
        $(".next").not(".disabled").click(function () {
            let pageNumber = parseInt($(".active>.page-link").text());
            mySelf.ajax_getTableData(getParamsForSearch(pageNumber, 1));
        })
    }
    Search(mySelf, getParamsForSearch) {
        $(".btnSearch").unbind();
        $(".btnSearch").click(function () {
            mySelf.ajax_getTableData(getParamsForSearch(1, 0));
        })
    }

    changePageNumber(mySelf) {
        $(".select2-selection--single.select2-selection__rendered.d-inline").unbind();
        $(".select2-selection--single.select2-selection__rendered.d-inline").change(function () {
            mySelf.ajax_getTableData(getParamsForSearch(1, 0));
        })
    }

    CheckUpdateAndFillInfo() {
        let myselft = this;
        $(".btnAdd").unbind();
        $(".btnEdit").unbind();
        $(".btnAdd").click(function () {
            myselft.index = $(this).data("index");
            myselft.update = false;
            myselft.setValueForCreate(myselft);
        });
        $(".btnEdit").click(function () {
            myselft.index = $(this).data("index");
            myselft.update = true;
            myselft.setValueForEdit(myselft, myselft.dataModel[myselft.index]);
        });
    }

    insertOrUpdate(url, callback, getParamsForSeach, reload) {
        let myself = this;
        $(".btnSave").unbind();
        $(".btnSave").click(function () {
            // Concat json
            let params;
            let modalValue = myself.getValueFromModal();
            if (modalValue != false) {
                if (callback != null) {
                    params = $.extend(modalValue, callback(myself.index));
                } else {
                    params = myself.getValueFromModal();
                }
                // End Concat json
                console.log(params)
                myself.ajax_Insert_Update(params, url, "Thành công", getParamsForSeach, reload);
            }

        });
    }

    // End  Set event



    // Support 
    setValueForCreate(myselft) {
        let modalItems = $("#modal_default").find("input,textarea");
        for (let i = 0; i < modalItems.length; i++) {
            if (modalItems[i].type == "checkbox") {
                modalItems[i].removeAttribute("checked");
                $("." + modalItems[i].id).parent().find('.switchery').prop('checked', false).trigger("click");
                $(".active-label").each(function () {
                    if ($(this).hasClass(modalItems[i].id)) {
                        $(this).text("Không kích hoạt");
                    }
                })
                continue;
            }
            modalItems[i].value = "";
        }
    }


    setValueForEdit(myselft, list) {
        let modalItems = $("#modal_default").find("input,textarea,select.form-control");
        debugger;
        for (let i = 0; i < modalItems.length; i++) {
            if (modalItems[i].type == "file") {
                modalItems[i].files = null;
                modalItems[i].value = null;
                modalItems[i].name = null;
                continue;
            } else if (modalItems[i].type == "checkbox") {
                console.log(list[modalItems[i].id])
                if (list[modalItems[i].id] == 1) {
                    $("." + modalItems[i].id).parent().find('.switchery').prop('checked', true).trigger("click");
                    $(".active-label").each(function () {
                        if ($(this).hasClass(modalItems[i].id)) {
                            $(this).text("Kích hoạt");
                        }
                    })
                } else {
                    $("." + modalItems[i].id).parent().find('.switchery').prop('checked', false).trigger("click");
                    $(".active-label").each(function () {
                        if ($(this).hasClass(modalItems[i].id)) {
                            $(this).text("Không kích hoạt");
                        }
                    })
                }
                continue;
            } else if ($("#" + modalItems[i].id).hasClass("pickatime")) {
                $("#" + modalItems[i].id).pickatime('picker').set('select', list[modalItems[i].id]);
                continue;
            } else if ($("#" + modalItems[i].id).hasClass("select") || $("#" + modalItems[i].id).hasClass("select-search")) {
                $("#" + modalItems[i].id).val(list[modalItems[i].id]).trigger("change");
            } else if ($("#" + modalItems[i].id).hasClass("pickadate")) {
                $("#" + modalItems[i].id).pickadate('picker').set('select', list[modalItems[i].id], { format: 'd/m/yyyy' });
                continue;
            }

            modalItems[i].value = list[modalItems[i].id];
            $(".currency").trigger("keyup");
        }
    }

    getValueFromModal() {
        let modalItems = $("#modal_default").find("input,textarea,select.form-control");
        let myself = this;
        //Parse value to json
        let params = "{";
        for (let i = 0; i < modalItems.length; i++) {
            if (modalItems[i].id != "") {
                if (modalItems[i].type == "checkbox") {
                    params += myself.getCheckBoxValue(modalItems[i]);
                    continue;
                }
                if (modalItems[i].type == 'file') {
                    params += "\"" + modalItems[i].id + "\": \"" + modalItems[i].name + "\",";
                    continue;
                }
                let res = myself.checkValid(modalItems[i]);
                if (res.valid) {
                    params += "\"" + modalItems[i].id + "\": \"" + modalItems[i].value.replace(/\n/g, "") + "\",";
                } else {
                    toastr.error(res.message);
                    return false;

                }
            }
        }
        params = params.substring(0, params.length - 1);
        params += "}";
        params = JSON.parse(params);
        return params;
    }

    checkValid(item) {

        if (item.classList.contains("not-empty")) {
            if (!this.checkEmpty(item)) {
                item.focus();
                return { valid: false, message: "Trường " + this.key[item.id] + " không được  trống" };
            }
        } else if (item.classList.contains("pickatime")) {

            if (item.value == "" || $("#PACKAGE_TIME_END_root .picker__list-item--highlighted").data("pick") < $("#PACKAGE_TIME_START_root .picker__list-item--highlighted").data("pick")) {
                return { valid: false, message: "Vui lòng chọn thời gian tập hợp lệ" };
            }

        }
        else if (item.classList.contains("pickadate")) {

            if (item.value == "" || $("#VOUCHER_BEGIN_table .picker__day--selected").data("pick") > $("#VOUCHER_END_table .picker__day--selected").data("pick")) {
                return { valid: false, message: "Vui lòng chọn thời gian tập hợp lệ" };
            }

        }
        else if (item.classList.contains("percent")) {
            if (parseFloat(item.value) > 100 || parseFloat(item.value) < 0) {
                return { valid: false, message: "Phần trăm phải nhỏ hơn 100" };
            }
        }

        return { valid: true }
    }

    static FormatCurency(money) {
        let regex = /\B(?=(\d{3})+(?!\d))/g;
        money = money.replace(regex, ",");
        return money;
    }
    FormatCurency(money) {
        let regex = /\B(?=(\d{3})+(?!\d))/g;
        money = money.replace(regex, ",");
        return money;
    }


    checkEmpty(item) {
        if (item.value.trim() != "") {
            return true;
        }
        return false;
    }

    getCheckBoxValue(item) {
        if ($("." + item.id).parent().find('.switchery').prop("checked")) {
            return "\"" + item.id + "\": \"" + 1 + "\",";
        } else {
            return "\"" + item.id + "\": \"" + 0 + "\",";
        }
    }

    formatJSONDate(jsonDate) {
        var date = new Date(parseInt(jsonDate.substr(6)));
        return date.toLocaleDateString('en-US');
    }

    // End Support 

    // Ajax
    Delete(url, getParamsForSeach) {
        let myself = this;
        $(".btnDelete").click(function () {
            if (confirm($(this).data("title") ? $(this).data("title") : 'Xóa?')) {
                let id = { id: $(this).data("id") };
                myself.ajax_Delete(id, url, "Xóa thành công", getParamsForSeach);
            }
        });
    }

    ajax_getTableData(params, className) {
        $.ajax({
            url: this.url_Get,
            type: "post",
            data: params,
        }).done(function (result) {
            if (className == undefined) {
                $(".data-table").html(result);
            } else {
                $("." + className).html(result);
            }

        });
    }

    ajax_Insert_Update(params, url, message, getParamsForSeach, reload) {
        let myself = this;
        $.ajax({
            url: url,
            type: "post",
            data: {
                value: params,
                isUpdate: myself.update
            },
        }).done(function (result) {
            if (result.Code == 0) {
                toastr.success(result.Result);
                $("#modal_default").modal("hide");
                if (reload != undefined) {
                    setTimeout(function () {
                        window.location.reload();
                    }, 1500);
                } else {
                    myself.ajax_getTableData(getParamsForSeach(1, 0));
                }
            } else {
                toastr.error(result.Result);
            }
        });
    }

    ajax_Delete(param, url, message, getParamsForSeach) {
        let myself = this;
        $.ajax({
            url: url,
            type: "post",
            data: param,
        }).done(function (result) {
            if (result) {
                toastr.success(message);
                myself.ajax_getTableData(getParamsForSeach(1, 0));
            } else {
                toastr.error("Đã có lỗi xảy ra");
            }
        });
    }
    // End Ajax
}