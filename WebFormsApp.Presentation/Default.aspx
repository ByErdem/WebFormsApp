<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebFormsApp.Presentation._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .hidden-id {
            display: none;
        }
    </style>



    <div class="container" style="margin-top: 20px;">

        <div class="container row">
            <button type="button" class="btn btn-success mt-3 mb-3" id="openNewRecordModalBtn">Create New Student</button>
        </div>

        <div style="border: 1px solid rgba(0,0,0,0.1); border-radius: 10px; margin-top: 20px; padding:10px; box-shadow: 2px 2px 5px rgba(0, 0, 0, 0.3);">
            <div class="row">
                <div class="col-md-4 mb-3">
                    <label for="uniqueIdInput">Unique ID:</label>
                    <input type="text" id="uniqueIdInput" class="form-control">
                </div>
                <div class="col-md-4 mb-3">
                    <label for="firstNameInput">First Name:</label>
                    <input type="text" id="firstNameInput" class="form-control">
                </div>
                <div class="col-md-4 mb-3">
                    <label for="lastNameInput">Last Name:</label>
                    <input type="text" id="lastNameInput" class="form-control">
                </div>
            </div>
            <div class="row">
                <div class="col-md-4 mb-3">
                    <label for="placeOfBirthInput">Place of Birth:</label>
                    <input type="text" id="placeOfBirthInput" class="form-control">
                </div>
                <div class="col-md-4 mb-3 d-flex align-items-end" style="margin-top: 25px;">
                    <button type="button" id="submitBtn" class="btn btn-primary">Search</button>
                </div>
            </div>
        </div>

        <table class="table table-bordered" style="margin-top: 25px;">
            <thead class="thead-light">
                <tr>
                    <th style="display: none">ID</th>
                    <th>Unique ID</th>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Birth Date</th>
                    <th>Place of Birth</th>
                    <th>Registration Date</th>
                    <th>Process</th>
                </tr>
            </thead>
            <tbody id="studentTableBody">
                <!-- Data rows will be appended here -->
            </tbody>
        </table>
        <ul id="pagination" class="pagination">
            <!-- Pagination links will be appended here -->
        </ul>
    </div>

    <!-- Yeni Kayıt Modal -->
    <div class="modal fade" id="newRecordModal" tabindex="-1" aria-labelledby="newRecordModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="newRecordModalLabel">Yeni Öğrenci Ekle</h5>
                </div>
                <div class="modal-body">
                    <form id="newRecordForm">
                        <div class="mb-3">
                            <label for="newUniqueId" class="form-label">Unique Id</label>
                            <input type="text" class="form-control" id="newUniqueId" required>
                        </div>
                        <div class="mb-3">
                            <label for="newFirstName" class="form-label">First Name</label>
                            <input type="text" class="form-control" id="newFirstName" required>
                        </div>
                        <div class="mb-3">
                            <label for="newLastName" class="form-label">Last Name</label>
                            <input type="text" class="form-control" id="newLastName" required>
                        </div>
                        <div class="mb-3">
                            <label for="newBirthDate" class="form-label">Birth Date</label>
                            <input type="date" class="form-control" id="newBirthDate" required>
                        </div>
                        <div class="mb-3">
                            <label for="newPlaceOfBirth" class="form-label">Place of Birth</label>
                            <input type="text" class="form-control" id="newPlaceOfBirth" required>
                        </div>
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="saveNewRecordBtn">Save</button>
                </div>
            </div>
        </div>
    </div>

    <script src="Scripts/Views/Default.js"></script>

</asp:Content>

