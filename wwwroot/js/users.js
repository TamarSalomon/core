const AllusersUrl = "/api/Allusers";
const userUrl = "/api/user"
users = [];
const token = localStorage.getItem("token");
const Authorization = `Bearer ${token}`;

getUsersList();


//Fetches the list of users from the server and displays them
function getUsersList() {
    fetch(AllusersUrl, {
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
        .then(data => displayUsers(data))
        .catch(error => {
            console.error('Unable to get users.', error);
        });
}

//Adds a new user to the server and updates the user list
function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const user = {
        id: 0,
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
        type: "user"

    };

    fetch(userUrl, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
        body: JSON.stringify(user)


    })
        .then(response =>
            response.json())
        .then(() => {
            getUsersList();
            addNameTextbox.value = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}

//Deletes a user with the specified id from the server and updates the user list
function deleteUser(id) {
    fetch(`${userUrl}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': Authorization
        },
    })
        .then(() => getUsersList())
        .catch(error => console.error('Unable to delete user.', error));
}


//Displays the users in a table based on the data received from the server
function displayUsers(usersList) {
    const usersBody = document.getElementById('Users');
    usersBody.innerHTML = '';

    displayCount(usersList.length);

    const button = document.createElement('button');

    usersList.forEach(user => {

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = usersBody.insertRow();

        let td1 = tr.insertCell(0);
        let textNodeId = document.createTextNode(user.id);
        td1.appendChild(textNodeId);

        let td2 = tr.insertCell(1);
        let textNodeName = document.createTextNode(user.name);
        td2.appendChild(textNodeName);

        let td3 = tr.insertCell(2);
        let textNodePassword = document.createTextNode(user.password);
        td3.appendChild(textNodePassword);

        let td4 = tr.insertCell(3);
        let textNodeType = document.createTextNode(user.type);
        td4.appendChild(textNodeType);

        let td5 = tr.insertCell(4);
        td5.appendChild(deleteButton);
    });

    users = usersList;
}

//Displays the number of users in the user list
function displayCount(userCount) {
    const name = (userCount === 1) ? 'user' : 'users';
    document.getElementById('counter').innerText = `There are ${userCount} ${name} in the user list`;
}



