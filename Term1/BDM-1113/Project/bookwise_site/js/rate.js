const API_URL = "https://webapimongodb01.azurewebsites.net/api"

//get parameters sent in url
const queryStr = new URLSearchParams(window.location.search);
const paramGet = Object.fromEntries(queryStr.entries());
const nickName = paramGet["txtNickName"];
const bookISBN = paramGet["isbn"];
const stateRead = "Read";

document.getElementById("txtNickName").value = nickName;

GetBookInfo()

function GetBookInfo() {
    
    fetch(`${API_URL}/BooksCtrl/GetInfo/${bookISBN}`).then(response => response.json()).then(books => {
      books.forEach(book => {
          
          document.getElementById("txtISBN").value = bookISBN
		  document.getElementById("txtISBN").classList.add('highlighted'); // Add a class;
          document.getElementById("txtName").value = book.name;
		  document.getElementById("txtName").classList.add('highlighted'); // Add a class;
          document.getElementById("txtYear").value = book.year;
		  document.getElementById("txtYear").classList.add('highlighted'); // Add a class;
          
          if (book.author[0] != null) {
             document.getElementById("txtAuthor1").value = book.author[0];
          }
          
          if (book.author[1] != null) {
             document.getElementById("txtAuthor2").value = book.author[1];
          }
          
          document.getElementById("txtPages").value = book.pages;     
		  document.getElementById("txtPages").classList.add('highlighted'); // Add a class;
          document.getElementById("txtSynopsis").value = book.synopsis;
		  document.getElementById("txtSynopsis").classList.add('highlighted'); // Add a class;
		  document.getElementById("txtComment").classList.add('highlighted'); // Add a class;
		 
      });
    });
      
  }

function rate_book() {
   
   const fecha    = new Date();
   const ano      = fecha.getFullYear();
   const mes      = fecha.getMonth() + 1;
   const dia      = fecha.getDate();
   const fechaStr = ano + "-" + ("0" + mes).slice(-2) + "-" + ("0" + dia).slice(-2);
   
   const rate = parseInt(document.getElementById("txtRate").value);
   const comment = document.getElementById("txtComment").value;
   
   
   let requestOpt = {};
   requestOpt.method = "PUT"

   fetch(`${API_URL}/UserBooksCtrl/Rate/${nickName}/${bookISBN}/${fechaStr}/${stateRead}/${rate}/${comment}`, requestOpt).then(res => res.json()).catch(error => console.error('Error:', error)).then(response => console.log('Success:', response));
   
   alert ("Book rate...OK")
}
