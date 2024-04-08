const todoUrl = '/api/todo';
const userUrl = '/api/user'
let tasks = [];
const token = localStorage.getItem("token");
const Authorization = `Bearer ${token}`;

getTasks();
IsAdmin();


//Fetches all the tasks from the server and displays them
function getTasks() {
    fetch(todoUrl, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },

    })
        .then(response => {
            if (response.status != 200) {
                throw new Error('Failed to fetch data');
            }
            return response.json();
        })
        .then(data => displayTasks(data))
        .catch(error => {
            console.error('Unable to get tasks.', error);
            window.location.href = "../index.html";
        });
}

//Fetches the current user's details from the server and populates the edit form
function getMyUser() {
    fetch(userUrl, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },

    })
        .then(response => {
            if (response.status != 200) {
                throw new Error('Failed to fetch data');
            }
            return response.json();
        })
        .then(data =>{
            document.getElementById('editUser-name').value =data.name;
            document.getElementById('editUser-password').value=data.password;
        })
        .catch(error => {
            console.error('Unable to get my user.', error);
        });
}


//Adds a new task and updates the task list
function addTask() {

    const addNameTextbox = document.getElementById('add-name');

    const task = {
        id: 0,
        name: addNameTextbox.value.trim(),
        isDone: false,
        userId: 0
    };

    fetch(todoUrl, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
        body: JSON.stringify(task)
    })
        .then(response => response.json())
        .then(() => {
            getTasks();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add task.', error));
}

//Updates a task with the values from the edit form on the server and updates the task list
function updateTask() {

    const taskId = document.getElementById('edit-id').value;
    const task = {
        Id: taskId,
        Name: document.getElementById('edit-name').value.trim(),
        IsDone: document.getElementById('edit-isDone').checked,
        UserId: 0
    };
    fetch(`${todoUrl}/${taskId}`, {

        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization

        },
        body: JSON.stringify(task)
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to update task.', error));

    closeInput();

    return false;
}

//Deletes a task with the specified id from the server and updates the task list
function deleteTask(id) {
    fetch(`${todoUrl}/${id}`, {

        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
    })
        .then(() => getTasks())
        .catch(error => console.error('Unable to delete task.', error));
}


//Displays the edit form for a task with the specified id
function displayEditForm(id) {
    const task = tasks.find(t => t.id === id);
    document.getElementById('edit-name').value = task.name;
    document.getElementById('edit-id').value = task.id;
    document.getElementById('edit-isDone').checked = task.isDone;
    document.getElementById('editForm').style.display = 'block';
}

//Closes the edit form
function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}



//Displays the tasks in the table based on the data received from the server
function displayTasks(data) {
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';

    displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(task => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = task.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${task.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteTask(${task.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(task.name);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    tasks = data;
}


//Displays the number of tasks in the task list
function displayCount(taskCount) {
    const name = ( taskCount === 1) ? 'task' : 'tasks';
    document.getElementById('counter').innerText = ` You have ${taskCount} ${name} on your task list! Successfully✨`;
}


//Checks if the current user is an admin and displays a link to the users page if true
function IsAdmin() {
    fetch('/Admin', {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
        body: JSON.stringify()
    })
        .then(res => {
            if (res.status === 200)
                usersLink();
        })
        .catch()
}

//Displays the link to the users page
const usersLink = () => {
    const linkToUsers = document.getElementById('forAdmin');
    linkToUsers.hidden = false;
}

//Displays the user details form
function displayUserDetails() {
    const UserDetails = document.getElementById('editUserForm');
    UserDetails.hidden = false;

}

//Closes the user details form
function closeeditUserInput() {
    document.getElementById('editUserForm').hidden = true;
}


//Updates the current user's details on the server and closes the user details form 
function updateUser() {
    getMyUser();
    const user = {
        Id: 0,
        Name: document.getElementById('editUser-name').value.trim(),
        Password: document.getElementById('editUser-password').value.trim(),
        Type: ""
    };
    fetch(userUrl, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
        body: JSON.stringify(user)
    })
        .then(response => {
            if (!response.ok) {
                throw new Error('Failed to update user');
            }        })
        .then(()=>
            {
                alert('update...');
                closeeditUserInput();

            }
         )
        .catch(error => {
            console.error('Unable to update user.', error);
            alert('Failed to update user. Please try again.');
        });
}
