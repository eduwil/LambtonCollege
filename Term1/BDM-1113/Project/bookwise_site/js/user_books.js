const API_URL = "https://webapimongodb01.azurewebsites.net/api"

//get parameters sent in url
const queryStr = new URLSearchParams(window.location.search);
//const url_keys = queryStr.keys();        // get names of fields
//const url_values = queryStr.values();    // get values of fields
//const url_entries = queryStr.entries();  // get key:value pair
const paramGet = Object.fromEntries(queryStr.entries());
const nickName = paramGet["txtNickName"];
const stateView = "Viewed";
const stateReading = "Reading";
const stateRead = "Read";

document.getElementById("userTitle").textContent = "Hi, " + nickName + "!";
document.getElementById("txtNickName").value = nickName;

/*
const lstGenres = []

for (const key of url_keys) {
     if (key != "txtNickName" && key != "txtYear" && key != "cboGenre") {
         lstGenres.push(key);
     };
}

console.log(lstGenres);
*/

GetUserBooks();
GetRecommendedBooks();

function GetUserBooks() {
    
    const HTMLResponse = document.querySelector("#userBooks");
    
    /*
    //columns headers
    HTMLResponse.appendChild(document.createElement("tr"));
        
    //ISBN
    let hdISBN = document.createElement("td");
    let stISBN = document.createElement("strong")
    stISBN.textContent = "ISBN";
    hdISBN.appendChild(stISBN);
    HTMLResponse.appendChild(hdISBN);
    */
    
    fetch(`${API_URL}/UserBooksCtrl/GetUserBooksList/${nickName}`).then(response => response.json()).then(books => {
    
        books.forEach(book => {
            HTMLResponse.appendChild(document.createElement("tr"));

            //ISBN
            let tdISBN = document.createElement("td");
            tdISBN.textContent = book.isbn;
            HTMLResponse.appendChild(tdISBN);

            //name
            let tdName = document.createElement("td");
            tdName.textContent = book.name;
            HTMLResponse.appendChild(tdName);

            //state
            let tdState = document.createElement("td");
            tdState.textContent = book.state;
            HTMLResponse.appendChild(tdState);

            //date
            let tdViewDate = document.createElement("td");
            
            switch (book.state) {
                case stateView:
                     tdViewDate.textContent = book.view_date;
                     break;
                default:
                     tdViewDate.textContent = book.reading_date;
                     break;
            }
            
            HTMLResponse.appendChild(tdViewDate);
            
            //read date
            let tdReadDate = document.createElement("td");
            tdReadDate.textContent = book.read_date;
            HTMLResponse.appendChild(tdReadDate);
            
            //rate
            let tdRate = document.createElement("td");
            if (book.state == stateRead) {
                tdRate.textContent = book.rate;
            }
            HTMLResponse.appendChild(tdRate);
            
            //id
            let tdId = document.createElement("td");
            
            switch (book.state) {
                case stateView:
                     let lnkView = document.createElement("a")
                     lnkView.href = "";
                     lnkView.onclick = updateState;
                     lnkView.class = "linkTable";
					 lnkView.isbn = book.isbn;					 
                     lnkView.textContent = "I am reading it!"
                     tdId.appendChild(lnkView);
                     break;
                case stateReading:
                     let lnkRate = document.createElement("a");
                     lnkRate.href = "rate.html?txtNickName=" + nickName + "&isbn=" + book.isbn;
                     lnkRate.class = "linkTable";
                     lnkRate.textContent = "Rate"
                     tdId.appendChild(lnkRate);
                     break;   
                default:
                     break;
            }
                 
            HTMLResponse.appendChild(tdId);           
        });
       
      });
}

function GetRecommendedBooks() {
    const HTMLResponse = document.querySelector("#recommendedBooks");
    
    fetch(`${API_URL}/BooksCtrl/GetRecommendedList/${nickName}`).then(response => response.json()).then(books => {
    
        books.forEach(book => {
            let cntRate = 0;
            let avgRate = 0;

            for (i in book.bookRate) {
                if (book.bookRate[i].state == stateRead) {
                    cntRate += 1;
                    avgRate += book.bookRate[i].rate;
                }
            }
            
            if (cntRate > 0) {
                avgRate = parseInt(avgRate / cntRate);
            }

            HTMLResponse.appendChild(document.createElement("tr"));

            //ISBN
            let tdISBN = document.createElement("td");
            tdISBN.textContent = book.isbn;
            HTMLResponse.appendChild(tdISBN);

            //name
            let tdName = document.createElement("td");
            tdName.textContent = book.name;
            HTMLResponse.appendChild(tdName);

            //year
            let tdYear = document.createElement("td");
            tdYear.textContent = book.year;
            HTMLResponse.appendChild(tdYear);

            //author
            let tdAuthor = document.createElement("td");
            tdAuthor.textContent = book.author[0];
            HTMLResponse.appendChild(tdAuthor);

            // Rates number
            let tdQty = document.createElement("td");
            tdQty.align = "center";
            if (cntRate > 0) {
                tdQty.textContent = cntRate;
            }
            HTMLResponse.appendChild(tdQty);
            
            // avg rate
            let tdAvg = document.createElement("td");

            if (cntRate > 0) {
                for (let i = 1; i <= avgRate; i++)
                {
                    let tdImg = document.createElement("img");
                    tdImg.src = "img/favs_24.ico";
                    tdAvg.appendChild(tdImg);
                }
            }

          //tdAvg.textContent = avgRate;
            HTMLResponse.appendChild(tdAvg);
             
            //book info
            let tdInfo = document.createElement("td");
            let lnkInfo = document.createElement("a")
            lnkInfo.href = "book.html?txtNickName=" + nickName + "&book_id=" + book.id;
            lnkInfo.class = "linkTable";
          //lnkInfo.target = "_blank"; --> to open in a new window
            lnkInfo.textContent = "Book info"
            tdInfo.appendChild(lnkInfo);
            HTMLResponse.appendChild(tdInfo);
            
            //book reviews
            let tdReviews = document.createElement("td");
            let lnkReviews = document.createElement("a")
            lnkReviews.href = "reviews.html?txtNickName=" + nickName + "&isbn=" + book.isbn + "&name=" + book.name;
            lnkReviews.class = "linkTable";
			lnkReviews.textContent = "Reviews"
            tdReviews.appendChild(lnkReviews);
            HTMLResponse.appendChild(tdReviews);
            
            /*
            let tdId2 = document.createElement("td");
            let btnView = document.createElement("button");
            btnView.onclick = open_win;
            btnView.textContent = "Ver"
            btnView.value = "Ver"
            btnView.id = "btn";
            btnView.name = book.id;
            
            tdId2.appendChild(btnView);
            HTMLResponse.appendChild(tdId2);
            */
        });
       
      });

}

function updateState() {
    
    const fecha    = new Date();
    const ano      = fecha.getFullYear();
    const mes      = fecha.getMonth() + 1;
    const dia      = fecha.getDate();
    const fechaStr = ano + "-" + ("0" + mes).slice(-2) + "-" + ("0" + dia).slice(-2);
    
    //"this" retrieve the element which invoked the onclick event
    const isbn = this.isbn;
    
    let requestOpt = {};
    requestOpt.method = "PUT"
    
    fetch(`${API_URL}/UserBooksCtrl/UpdateState/${nickName}/${isbn}/${fechaStr}/${stateReading}`, requestOpt).then(res => res.json()).catch(error => console.error('Error:', error)).then(response => console.log('Success:', response));
    
    alert ("State updated...OK")
    
    location.reload();
}

function open_win() {
    //https://desarrolloweb.com/articulos/18.php
    //https://htmlcolorcodes.com/es/nombres-de-los-colores/
    
    URL = "book.html?txtNickName=" + nickName + "&book_id=" + btn.name;
    window.open(URL, "ventana1", "width=120, height=300, scrollbars=NO") 
}





