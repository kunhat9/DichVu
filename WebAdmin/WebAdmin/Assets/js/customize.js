try {
    toastr.options = {
        "closeButton": false,
        "debug": false,
        "newestOnTop": false,
        "progressBar": false,
        "positionClass": "toast-top-right",
        "preventDuplicates": false,
        "onclick": null,
        "showDuration": "300",
        "hideDuration": "1000",
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut"
    }
} catch (e) {

}

function checkNumber(evt) {
    if (evt.keyCode > 47 && evt.keyCode < 58) {
        return true;
    }
    return false;
}

$(document).ready(function () {
    let elems = document.querySelectorAll('.switchery');

    if (elems.length != 0) {
        for (let i = 0; i < elems.length; i++) {
            var switchery = new Switchery(elems[i], { className: "switchery " + elems[i].id });
        }
    }

    $('.pickadate').pickadate({
        selectMonths: true,
        selectYears: true,
        format: "dd/mm/yyyy"
    });

    $('.pickatime').pickatime();
    function setActive() {
        let baseUrl = location.pathname;
        let url = baseUrl.split("/");
        let elements = $('a[href*="' + url[2]  + '"]');
        if (elements.length == 0) {
            elements = $('a[href*="' + url[1] + '"]');
            for (var i = 0; i < elements.length; i++) {
                if (elements[i] && elements[i].href.includes(url[1])) {
                    setActiveAddClass(elements[i]);
                    break;
                }
            }
        } else {
            for (var i = 0; i < elements.length; i++) {
                if (elements[i] && elements[i].href.endsWith(baseUrl)) {
                    elements[i].parentElement.className = "active";
                    setActiveAddClass(elements[i]);
                }
            }
        }
        
    }
    function setActiveAddClass(e) {
        let p = e.parentElement.parentElement;
        p.parentElement.classList.add("active");
        if (p.classList.contains("hidden-ul")) {
            p.classList.remove("hidden-ul");
            setActiveAddClass(p);
        }
    }

    function FormatCurency(money) {
        let regex = /\B(?=(\d{3})+(?!\d))/g;
        money = money.replace(regex, ",");
        return money;
    }

    //Format currency 
    $(".currency").keyup(function () {
        let money = $(this).val().replace(/\,/g, '').toString();
        $(this).val(FormatCurency(money));
    });

    //change status of checkbox
    $(".switchery").click(function () {
        let className = $(this).attr('id');
        if ($(this).prop('checked')) {
            $(".active-label").each(function () {
                if ($(this).hasClass(className)) {
                    $(this).text("Kích hoạt");
                }
            })
        } else {
            $(".active-label").each(function () {
                if ($(this).hasClass(className)) {
                    $(this).text("Không kích hoạt");
                }
            })

        }
    })

    setActive();
});

function fnExcelReport() {
    var tab_text = "<table border='2px'><tr bgcolor='#87AFC6'>";
    var textRange; var j = 0;
    tab = document.getElementsByClassName('table-fixed')[0]; // class of table

    for (j = 0 ; j < tab.rows.length ; j++) {
        tab_text = tab_text + tab.rows[j].innerHTML + "</tr>";
        //tab_text=tab_text+"</tr>";
    }

    tab_text = tab_text + "</table>";
    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");//remove if u want links in your table
    tab_text = tab_text.replace(/<img[^>]*>/gi, ""); // remove if u want images in your table
    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, ""); // reomves input params

    var ua = window.navigator.userAgent;
    var msie = ua.indexOf("MSIE ");

    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./))      // If Internet Explorer
    {
        txtArea1.document.open("txt/html", "replace");
        txtArea1.document.write(tab_text);
        txtArea1.document.close();
        txtArea1.focus();
        sa = txtArea1.document.execCommand("SaveAs", true, "Say Thanks to Sumit.xls");
    }
    else                 //other browser not tested on IE 11
        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

    return (sa);
}
