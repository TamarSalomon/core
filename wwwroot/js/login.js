const loginUrl = "/api/login";

 if (localStorage.getItem('token')) {
     window.location.href = "tasks.html";
 }


//Retrieves user input and initiates login process
function getDetailsForLogin() {
    var name = document.getElementById('name').value;
    var password = document.getElementById('password').value;
    login(name, password);
}


//Sends login request to the server and handles the response
function login(name, password) {
    fetch(loginUrl, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({
            Id: 0,
            Name: name,
            Password: password,
            Type: "user"
        })
    })
        .then(response => response.json())
        .then(data => {
            saveToken(data)
        })

        .catch(
            console.log("the user is not registered in the system, please contact the manager"))
            
}

//Saves the token received from the server to the local storage and redirects the user to the home page
function saveToken(token) {
    localStorage.setItem("token", token);
    var homePagePath = "tasks.html";
    window.location.href = homePagePath;
}

//Processes Google Sign-In response
function handleCredentialResponse(response) {
    if (response.credential) {
        var idToken = response.credential;
        var decodedToken = parseJwt(idToken);
        var userName = decodedToken.name;
        var userPassword = decodedToken.sub;
        login(userName, userPassword);
    } else {
        alert('Google Sign-In was cancelled.');
    }
}


//Parses JWT token from Google Sign-In
function parseJwt(token) {
    var base64Url = token.split('.')[1];
    var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    var jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));

    return JSON.parse(jsonPayload);
}


