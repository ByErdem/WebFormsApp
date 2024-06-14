$(document).ready(function () {

    // Yeni kayıt modalını açma
    $("#openNewRecordModalBtn").click(function () {
        $("#newRecordModal").modal('show');
    });

    // Yeni kaydı kaydetme
    $("#saveNewRecordBtn").click(function () {
        const studentDto = {
            UniqueId: $("#newUniqueId").val(),
            FirstName: $("#newFirstName").val(),
            LastName: $("#newLastName").val(),
            BirthDate: new Date($("#newBirthDate").val()).toISOString(),
            PlaceOfBirth: $("#newPlaceOfBirth").val()
        };

        $.ajax({
            type: "POST",
            url: "Default.aspx/Add",
            data: JSON.stringify({ dto: studentDto }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                if (response.d.Result.ResultStatus == 0) {
                    $("#newRecordModal").modal('hide');
                    fetchPageData(1); // Yeni kaydı ekledikten sonra tabloyu güncelle
                }
                else {
                    console.error(response.d.Result.ErrorMessages);
                }
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    $("#submitBtn").click(function () {
        fetchPageData(1);
    });
});

const pageSize = 5;
let currentPage = 1;
let totalItems = 0;

function fetchPageData(pageNumber) {
    var studentDto = {
        Id: 0,
        UniqueId: $("#uniqueIdInput").val(),
        FirstName: $("#firstNameInput").val(),
        LastName: $("#lastNameInput").val(),
        BirthDate: null,
        PlaceOfBirth: $("#placeOfBirthInput").val(),
        PageNumber: pageNumber,
        PageSize: pageSize
    };

    $.ajax({
        type: "POST",
        url: "Default.aspx/GetAll",
        data: JSON.stringify({ dto: studentDto }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log(response);
            totalItems = response.d.Result.TotalRecords; // Toplam kayıt sayısını sunucudan alıyoruz
            const jsonData = response.d.Result.Data; // Verileri alıyoruz
            renderTable(jsonData);
            renderPagination();
        },
        error: function (xhr, status, error) {
            console.error(xhr.responseText);
        }
    });
}

function padToTwoDigits(num) {
    return num.toString().padStart(2, '0');
}

function formatDateString(date) {
    const day = padToTwoDigits(date.getDate());
    const month = padToTwoDigits(date.getMonth() + 1); // Ay 0-11 arası olduğu için 1 ekliyoruz
    const year = date.getFullYear();
    return `${day}/${month}/${year}`;
}

function renderTable(data) {
    const tableBody = $('#studentTableBody');
    tableBody.empty();

    data.forEach(item => {
        const birthDate = new Date(parseInt(item.BirthDate.substr(6)));
        const registrationDate = new Date(parseInt(item.RegistrationDateTime.substr(6)));

        tableBody.append(`
            <tr data-id="${item.Id}">
                <td class="hidden-id">${item.Id}</td>
                <td class="uniqueId">${item.UniqueId}</td>
                <td class="firstName">${item.FirstName}</td>
                <td class="lastName">${item.LastName}</td>
                <td class="birthDate">${formatDateString(birthDate)}</td>
                <td class="placeOfBirth">${item.PlaceOfBirth}</td>
                <td class="registrationDate">${formatDateString(registrationDate)}</td>
                <td style="width:100px;">
                    <button type="button" class="btn btn-link editBtn"><i class="fa fa-pencil"></i></button>
                    <button type="button" class="btn btn-link cancelBtn" style="display:none;"><i class="fa fa-times"></i></button>
                    <button type="button" class="btn btn-link deleteBtn"><i class="fa fa-trash"></i></button>
                    <button type="button" class="btn btn-link saveBtn" style="display:none;"><i class="fa fa-save"></i></button>
                </td>
            </tr>
        `);
    });

    $('.editBtn').click(function () {
        const row = $(this).closest('tr');
        row.find('.editBtn').hide();
        row.find('.deleteBtn').hide();
        row.find('.saveBtn, .cancelBtn').show();
        row.find('td').each(function () {
            const cell = $(this);
            const cellText = cell.text();
            if (cell.hasClass('uniqueId') || cell.hasClass('firstName') || cell.hasClass('lastName') || cell.hasClass('birthDate') || cell.hasClass('placeOfBirth') || cell.hasClass('registrationDate')) {
                cell.data('original', cellText);
                cell.html(`<input type="text" class="form-control" value="${cellText}">`);
            }
        });
    });

    $('.saveBtn').click(function () {
        console.log("deneme");
        const row = $(this).closest('tr');

        const id = row.data('id');
        const uniqueId = row.find('.uniqueId input').val();
        const firstName = row.find('.firstName input').val();
        const lastName = row.find('.lastName input').val();
        const birthDate = row.find('.birthDate input').val();
        const placeOfBirth = row.find('.placeOfBirth input').val();
        const registrationDate = row.find('.registrationDate input').val();

        const studentDto = {
            Id: id,
            UniqueId: uniqueId,
            FirstName: firstName,
            LastName: lastName,
            BirthDate: formatAndValidateDate(birthDate),
            PlaceOfBirth: placeOfBirth,
            RegistrationDateTime: formatAndValidateDate(registrationDate)
        };

        console.log(studentDto)

        $.ajax({
            type: "POST",
            url: "Default.aspx/Update",
            data: JSON.stringify({ dto: studentDto }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                row.find('td').each(function () {
                    const cell = $(this);
                    if (cell.hasClass('uniqueId') || cell.hasClass('firstName') || cell.hasClass('lastName') || cell.hasClass('birthDate') || cell.hasClass('placeOfBirth') || cell.hasClass('registrationDate')) {
                        const input = cell.find('input');
                        if (input.length) {
                            cell.text(input.val());
                        }
                    }
                });
                row.find('.editBtn').show();
                row.find('.deleteBtn').show();
                row.find('.saveBtn, .cancelBtn').hide();
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });

    $('.cancelBtn').click(function () {
        const row = $(this).closest('tr');
        row.find('td').each(function () {
            const cell = $(this);
            if (cell.hasClass('uniqueId') || cell.hasClass('firstName') || cell.hasClass('lastName') || cell.hasClass('birthDate') || cell.hasClass('placeOfBirth') || cell.hasClass('registrationDate')) {
                const originalText = cell.data('original');
                cell.text(originalText);
            }
        });
        row.find('.editBtn').show();
        row.find('.deleteBtn').show();
        row.find('.saveBtn, .cancelBtn').hide();
    });

    $('.deleteBtn').click(function () {
        const row = $(this).closest('tr');
        const id = row.data('id');

        $.ajax({
            type: "POST",
            url: "Default.aspx/Delete",
            data: JSON.stringify({ id: id }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {
                console.log(response);
                row.remove();
            },
            error: function (xhr, status, error) {
                console.error(xhr.responseText);
            }
        });
    });
}

function formatAndValidateDate(dateString) {
    function isValidDate(dateString) {
        const date = new Date(dateString);
        return date instanceof Date && !isNaN(date);
    }

    let formattedDate;

    if (dateString) {
        // Örnek: DD/MM/YYYY formatından YYYY-MM-DD formatına dönüştürmek
        const [day, month, year] = dateString.split('/');
        formattedDate = `${year}-${month}-${day}`;

        if (isValidDate(formattedDate)) {
            return new Date(formattedDate).toISOString();
        } else {
            throw new Error("Invalid date value");
        }
    } else {
        throw new Error("Date input is empty");
    }
}

function renderPagination() {
    const totalPages = Math.ceil(totalItems / pageSize);
    const pagination = $('#pagination');
    pagination.empty();

    for (let i = 1; i <= totalPages; i++) {
        pagination.append(`
            <li class="page-item ${i === currentPage ? 'active' : ''}">
                <a class="page-link" href="#" data-page="${i}">${i}</a>
            </li>
        `);
    }

    $('.page-link').click(function (e) {
        e.preventDefault();
        const page = $(this).data('page');
        currentPage = page;
        fetchPageData(page);
    });
}

fetchPageData(1);