function editProfile() {
    // Сделать поля редактируемыми
    document.getElementById('firstName').readOnly = false;
    document.getElementById('lastName').readOnly = false;
    document.getElementById('email').readOnly = false;
    document.getElementById('gmina').readOnly = false;

    // Показать кнопки Save и Cancel, скрыть кнопку Edit
    document.getElementById('save').hidden = false;
    document.getElementById('cancel').hidden = false;
    document.getElementById('edit').hidden = true;

    // Показать форму редактирования
    document.getElementById('editProfileForm').style.display = 'block';
}

function cancelEdit() {
    // Сделать поля только для чтения
    document.getElementById('firstName').readOnly = true;
    document.getElementById('lastName').readOnly = true;
    document.getElementById('email').readOnly = true;
    document.getElementById('gmina').readOnly = true;

    // Скрыть кнопки Save и Cancel, показать кнопку Edit
    document.getElementById('save').hidden = true;
    document.getElementById('cancel').hidden = true;
    document.getElementById('edit').hidden = false;

    // Скрыть форму редактирования
    document.getElementById('editProfileForm').style.display = 'block';
}

document.getElementById('editProfileForm').addEventListener('submit', function(event) {
    event.preventDefault(); // Предотвратить отправку формы

    // Получить данные из формы редактирования
    var formData = {
        firstName: document.getElementById('firstName').value,
        lastName: document.getElementById('lastName').value,
        email: document.getElementById('email').value,
        gmina: document.getElementById('gmina').value
    };

    // Обновить текущие значения профиля на странице (опционально)
    document.getElementById('firstName').value = formData.firstName;
    document.getElementById('lastName').value = formData.lastName;
    document.getElementById('email').value = formData.email;
    document.getElementById('gmina').value = formData.gmina;

    // Сохранение данных на сервер (замените этот код на реальную отправку данных)
    console.log('Отправка данных на сервер:', formData);
    cancelEdit();
});
