const API_URL = "https://webapimongodb01.azurewebsites.net/api"

//get parameters sent in url
const queryStr = new URLSearchParams(window.location.search);
const paramGet = Object.fromEntries(queryStr.entries());
const nickName = paramGet["txtNickName"];
const bookId   = paramGet["book_id"];

let bookExists = false;
let bookISBN   = "";
let bookName   = "";

document.getElementById("txtNickName").value = nickName;

GetGenres()
GetBookInfo()

function GetGenres() {

    fetch(`${API_URL}/GenresCtrl/GetList`).then(response => response.json()).then(genres => {
      const HTMLResponse = document.querySelector("#lstGenres");      
      let tdCol = document.createElement("td");
      letCon = 0;
      
      genres.forEach(genre => {
        
         if (letCon % 5 == 0) {
             tdCol = document.createElement("td");
             HTMLResponse.appendChild(tdCol)
         }
         
         letCon += 1;
         
         let chkItem = document.createElement("input");
         chkItem.type = "checkbox"
         chkItem.id = genre.name;
         chkItem.name = genre.name;
         chkItem.value = genre.name;
         chkItem.className = "chkLstGen";
         tdCol.appendChild(chkItem);
         
         let lblItem = document.createElement("label");
         lblItem.textContent = genre.name;
         tdCol.appendChild(lblItem);
     
         let brItem = document.createElement("br");
         tdCol.appendChild(brItem);
      });
     
    });
}

function GetBookInfo() {

  fetch(`${API_URL}/BooksCtrl/GetInfoById/${bookId}`).then(response => response.json()).then(books => {
    books.forEach(book => {
        bookExists = true;
        bookISBN = book.isbn;
        bookName = book.name;

        document.getElementById("txtISBN").value = book.isbn;
        document.getElementById("txtName").value = book.name;
        document.getElementById("txtYear").value = book.year;
        
        if (book.author[0] != null) {
           document.getElementById("txtAuthor1").value = book.author[0];
        }
        
        if (book.author[1] != null) {
           document.getElementById("txtAuthor2").value = book.author[1];
        }
        
        document.getElementById("txtPages").value = book.pages;
        
        for (i in book.languages) {
             if (document.getElementById(book.languages[i]) !== null) {
                 document.getElementById(book.languages[i]).checked = true;
				 document.getElementById(book.languages[i]).classList.add('chkLstInt'); // Add this line
             }
        }
        
        document.getElementById("txtSynopsis").value = book.synopsis;

        for (i in book.genre) {
             if (document.getElementById(book.genre[i]) != null) {
                 document.getElementById(book.genre[i]).checked = true;
				 document.getElementById(book.genre[i]).classList.add('chkLstInt'); // Add this line
             }
        }
        
        const HTMLResponse = document.querySelector("#lstRelated");
        
        for (i in book.related) {
             let trRow = document.createElement("tr")
             
             let tdCol_isbn = document.createElement("td");
             tdCol_isbn.textContent = book.related[i].isbn;
             trRow.appendChild(tdCol_isbn)
             
             let tdCol_name = document.createElement("td");
             tdCol_name.textContent = book.related[i].name;
             trRow.appendChild(tdCol_name);
             
             HTMLResponse.appendChild(trRow);
        }

    });
  });
    
}

function save_user_books(pRead) {

    //Date
    //https://www.freecodecamp.org/espanol/news/javascript-date-now-como-obtener-la-fecha-actual-con-javascript/
    //https://desarrolloweb.com/articulos/mostrar-fecha-actual-javascript.html
    const fecha    = new Date();
    const ano      = fecha.getFullYear();
    const mes      = fecha.getMonth() + 1;
    const dia      = fecha.getDate();
    const fechaStr = ano + "-" + ("0" + mes).slice(-2) + "-" + ("0" + dia).slice(-2);
     
    let requestHead = new Headers();
    requestHead.append("Content-Type", "application/json");

    let userBook = {};
    userBook.nickname = nickName;
    userBook.ISBN = bookISBN;
    userBook.name = bookName;
    userBook.view_date = fechaStr;
    
    if (pRead) {
        userBook.state = "Reading";
        userBook.reading_date = fechaStr;
    } else {
        userBook.state = "Viewed";
        userBook.reading_date = "";
    }

    userBook.read_date = ""
    userBook.rate = 0
    userBook.comment = ""
    
    let requestOpt = {};
    requestOpt.method = "POST"
    requestOpt.body = JSON.stringify(userBook);
    requestOpt.headers = requestHead;

    fetch(`${API_URL}/UserBooksCtrl/Add`, requestOpt).then(res => res.json()).catch(error => console.error('Error:', error)).then(response => console.log('Success:', response));
    
    alert ("Book added to the user list!")
 }
 