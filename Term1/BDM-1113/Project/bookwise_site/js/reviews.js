const API_URL = "https://webapimongodb01.azurewebsites.net/api"

//get parameters sent in url
const queryStr = new URLSearchParams(window.location.search);
const paramGet = Object.fromEntries(queryStr.entries());
const nickName = paramGet["txtNickName"];
const bookISBN = paramGet["isbn"];
const bookName = paramGet["name"];
const stateRead = "Read";

document.getElementById("bookTitle").textContent = bookName;
document.getElementById("txtNickName").value = nickName;

GetReviews()

function GetReviews() {
    
    fetch(`${API_URL}/UserBooksCtrl/GetReviews/${bookISBN}/${stateRead}`).then(response => response.json()).then(books => {
        const HTMLResponse = document.querySelector("#bookReviews");  

        books.forEach(book => {
            tbl = document.createElement("table");
            tbl.border = 1
            
            trRevNick = document.createElement("tr");
            tdNickLbl = document.createElement("td");
            tdNickLbl.textContent = "Nickname:"
            tdNickVal = document.createElement("td");
            tdNickVal.textContent = book.nickname;
            trRevNick.appendChild(tdNickLbl);
            trRevNick.appendChild(tdNickVal);
            
            trRevRate = document.createElement("tr");
            tdRateLbl = document.createElement("td");
            tdRateLbl.textContent = "Rate:"
            tdRateVal = document.createElement("td");
            tdRateVal.textContent = book.rate;
            trRevRate.appendChild(tdRateLbl);
            trRevRate.appendChild(tdRateVal);

            trRevComment = document.createElement("tr");
            tdCommentLbl = document.createElement("td");
            tdCommentLbl.textContent = "Comment:"
            tdCommentVal = document.createElement("td");
            tdCommentVal.textContent = book.comment;
            trRevComment.appendChild(tdCommentLbl);
            trRevComment.appendChild(tdCommentVal);

            tbl.appendChild(trRevNick);
            tbl.appendChild(trRevRate);
            tbl.appendChild(trRevComment);
            HTMLResponse.appendChild(tbl);
            HTMLResponse.appendChild(document.createElement("br"))
        })
    })
}

