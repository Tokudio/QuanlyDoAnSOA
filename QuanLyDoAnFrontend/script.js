
const API_URL = "https://localhost:7273/api/SinhVien";

const studentTable = document.getElementById("studentTable");
const searchInput = document.getElementById("searchInput");
const message = document.getElementById("message");
const modal = document.getElementById("studentModal");
const studentForm = document.getElementById("studentForm");

let students = [];
let isEditing = false;

// Lấy giá trị thuộc tính, hỗ trợ cả camelCase và PascalCase
function getField(student, camel, pascal) {
    return student[camel] ?? student[pascal] ?? "";
}

// Tải danh sách sinh viên
async function loadStudents() {
    message.textContent = "";
    studentTable.innerHTML =
        '<tr><td colspan="6">Đang tải dữ liệu...</td></tr>';

    try {
        const response = await fetch(API_URL);

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        students = await response.json();
        searchStudents();
    } catch (error) {
        console.error("Lỗi tải sinh viên:", error);
        message.style.color = "#dc2626";
        message.textContent =
            "Không thể tải sinh viên. Hãy kiểm tra API và CORS.";

        studentTable.innerHTML =
            '<tr><td colspan="6">Không có dữ liệu</td></tr>';
    }
}

// Hiển thị danh sách sinh viên
function renderStudents(data) {
    studentTable.innerHTML = "";

    if (!Array.isArray(data) || data.length === 0) {
        studentTable.innerHTML =
            '<tr><td colspan="6">Chưa có sinh viên</td></tr>';
        return;
    }

    data.forEach(student => {
        const maSV = getField(student, "maSV", "MaSV");
        const hoTen = getField(student, "hoTen", "HoTen");
        const ngaySinh = getField(student, "ngaySinh", "NgaySinh");
        const lop = getField(student, "lop", "Lop");
        const email = getField(student, "email", "Email");

        const row = document.createElement("tr");

        const values = [
            maSV,
            hoTen,
            ngaySinh ? String(ngaySinh).slice(0, 10) : "",
            lop,
            email
        ];

        values.forEach(value => {
            const cell = document.createElement("td");
            cell.textContent = value;
            row.appendChild(cell);
        });

        // Cột thao tác
        const actionCell = document.createElement("td");

        const editBtn = document.createElement("button");
        editBtn.textContent = "Sửa";
        editBtn.type = "button";
        editBtn.addEventListener("click", () => editStudent(student));

        const deleteBtn = document.createElement("button");
        deleteBtn.textContent = "Xóa";
        deleteBtn.type = "button";
        deleteBtn.className = "secondary";
        deleteBtn.style.marginLeft = "6px";
        deleteBtn.addEventListener("click", () => deleteStudent(maSV));

        actionCell.append(editBtn, deleteBtn);
        row.appendChild(actionCell);

        studentTable.appendChild(row);
    });
}

// Tìm kiếm sinh viên
function searchStudents() {
    const keyword = searchInput.value.trim().toLowerCase();

    const filtered = students.filter(student => {
        const maSV = String(getField(student, "maSV", "MaSV"));
        const hoTen = String(getField(student, "hoTen", "HoTen"));

        return maSV.toLowerCase().includes(keyword) ||
               hoTen.toLowerCase().includes(keyword);
    });

    renderStudents(filtered);
}

// Mở form thêm sinh viên
document.getElementById("addBtn").addEventListener("click", () => {
    isEditing = false;
    studentForm.reset();

    document.getElementById("maSV").readOnly = false;
    document.querySelector("#studentModal h2").textContent =
        "Thêm sinh viên";
    studentForm.querySelector('[type="submit"]').textContent =
        "Lưu sinh viên";

    modal.classList.remove("hidden");
});

// Mở form sửa sinh viên
function editStudent(student) {
    isEditing = true;
    studentForm.reset();

    document.querySelector("#studentModal h2").textContent =
        "Chỉnh sửa sinh viên";

    document.getElementById("maSV").value =
        getField(student, "maSV", "MaSV");
    document.getElementById("maSV").readOnly = true;

    document.getElementById("hoTen").value =
        getField(student, "hoTen", "HoTen");

    const ngaySinh = getField(student, "ngaySinh", "NgaySinh");
    document.getElementById("ngaySinh").value =
        ngaySinh ? String(ngaySinh).slice(0, 10) : "";

    document.getElementById("lop").value =
        getField(student, "lop", "Lop");

    document.getElementById("email").value =
        getField(student, "email", "Email");

    studentForm.querySelector('[type="submit"]').textContent =
        "Lưu thay đổi";

    modal.classList.remove("hidden");
}

// Đóng form
document.getElementById("closeModal").addEventListener("click", () => {
    modal.classList.add("hidden");
});

// Thêm hoặc cập nhật sinh viên
studentForm.addEventListener("submit", async (event) => {
    event.preventDefault();

    const student = {
        maSV: document.getElementById("maSV").value.trim(),
        hoTen: document.getElementById("hoTen").value.trim(),
        ngaySinh: document.getElementById("ngaySinh").value,
        lop: document.getElementById("lop").value.trim(),
        email: document.getElementById("email").value.trim()
    };

    const editing = isEditing;
    const url = editing
        ? `${API_URL}/${encodeURIComponent(student.maSV)}`
        : API_URL;

    try {
        const response = await fetch(url, {
            method: editing ? "PUT" : "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(student)
        });

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || `HTTP ${response.status}`);
        }

        modal.classList.add("hidden");
        studentForm.reset();

        await loadStudents();

        message.style.color = "green";
        message.textContent = editing
            ? "Cập nhật sinh viên thành công!"
            : "Thêm sinh viên thành công!";
    } catch (error) {
        console.error("Lỗi lưu sinh viên:", error);
        message.style.color = "#dc2626";
        message.textContent =
            "Không thể lưu sinh viên: " + error.message;
    }
});

// Xóa sinh viên
async function deleteStudent(maSV) {
    const confirmed = confirm(
        `Cậu có chắc muốn xóa sinh viên ${maSV} không?`
    );

    if (!confirmed) return;

    try {
        const response = await fetch(
            `${API_URL}/${encodeURIComponent(maSV)}`,
            { method: "DELETE" }
        );

        if (!response.ok) {
            const errorText = await response.text();
            throw new Error(errorText || `HTTP ${response.status}`);
        }

        await loadStudents();

        message.style.color = "green";
        message.textContent = "Xóa sinh viên thành công!";
    } catch (error) {
        console.error("Lỗi xóa sinh viên:", error);
        message.style.color = "#dc2626";
        message.textContent =
            "Không thể xóa sinh viên: " + error.message;
    }
}

// Các sự kiện tìm kiếm và tải lại
document.getElementById("searchBtn")
    .addEventListener("click", searchStudents);

searchInput.addEventListener("input", searchStudents);

document.getElementById("reloadBtn")
    .addEventListener("click", loadStudents);

// Tải dữ liệu lần đầu
loadStudents();