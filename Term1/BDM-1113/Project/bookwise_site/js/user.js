const API_URL = "https://webapimongodb01.azurewebsites.net/api"

//get parameters sent in url
const queryStr = new URLSearchParams(window.location.search);
const paramGet = Object.fromEntries(queryStr.entries());
const nickName = paramGet["txtNickName"];
let userExists = false;
let userId = ""

document.getElementById("userTitle").textContent = "Hi, " + nickName + "!";
document.getElementById("txtNickName").value = nickName;

getGenres();
//alert ("Welcome, " + nickName + "!");
//setTimeout(() => {console.log("1 Segundo esperado")}, 5000);
getUserInfo();

function getGenres() {

    fetch(`${API_URL}/GenresCtrl/GetList`).then(response => response.json()).then(genres => {
      const HTMLResponse = document.querySelector("#lstInterest");
      let tdCol = document.createElement("td");
      letCon = 0;
      
      genres.forEach(genre => {
        
         if (letCon % 10 == 0) {
             tdCol = document.createElement("td");
             HTMLResponse.appendChild(tdCol)
         }
         
         letCon += 1;
		 
		 let chkItem = document.createElement("input");
         chkItem.type = "checkbox"
         chkItem.id = genre.name;
         chkItem.name = genre.name;
         chkItem.value = genre.name;
         chkItem.className = "chkLstInt";
         tdCol.appendChild(chkItem);
         
         let lblItem = document.createElement("label");
         lblItem.textContent = genre.name;
         tdCol.appendChild(lblItem);
     
         let brItem = document.createElement("br");
         tdCol.appendChild(brItem);
      });
     
    });
}

function getUserInfo() {

    fetch(`${API_URL}/UsersCtrl/GetInfo/${nickName}`).then((response) => response.json()).then((users) => {
        users.forEach(user => {
            userExists = true;
            userId = user.id;
            document.getElementById("txtYear").value = user.birth_year;
            document.getElementById("cboGenre").value = user.genre;
           
            for (i in user.interests) {
                if (document.getElementById(user.interests[i]) !== null) {
                    document.getElementById(user.interests[i]).checked = true
                }
            }

        });
    });

}

function saveUser() {
   
   //get interests
   const itemInt = document.getElementsByClassName("chkLstInt");
   const usrLstInt = new Array ();
    
   for (i in itemInt) {
        if (itemInt[i].type == "checkbox") {
            if (itemInt[i].checked) {
                usrLstInt.push(itemInt[i].name)
            }
        }
   };
   
   let requestOpt = {};
   let user = {};
   let action = "";

   if (userExists) {
       user.id = userId;
       requestOpt.method = "PUT"
       action = "Replace"
   } else {
       requestOpt.method = "POST"
       action = "Add"
   }
   
   user.nickname = nickName;
   user.birth_year = parseInt(document.getElementById("txtYear").value);
   user.genre = document.getElementById("cboGenre").value;
   user.interests = usrLstInt

   let requestHead = new Headers();
   requestHead.append("Content-Type", "application/json");

   requestOpt.body = JSON.stringify(user);
   requestOpt.headers = requestHead;

   fetch(`${API_URL}/UsersCtrl/${action}`, requestOpt).then(res => res.json()).catch(error => console.error('Error:', error)).then(response => console.log('Success:', response));

   alert('Your preferences have been successfully saved!\nThank you for choosing us!');
}
