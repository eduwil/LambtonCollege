
function clearCtrls()
{
   for (var i = 0; i < 16; i++)
   {
	   numImg = i.toString().padStart(2, '0');
       let divImg = document.getElementById("img" + numImg);
       divImg.src = "";
	   divImg.style.visibility = "hidden";
   }
   
   const canvas = document.getElementById("imgCanvas");
   const ctx = canvas.getContext("2d");
   ctx.clearRect(0, 0, 480, 480)
   
   const lblResult = document.getElementById("lblResult");
   lblResult.innerHTML = "";
}

function changeDir()
{
	clearCtrls();
	
	const dirImg = document.getElementById("dirImages");
   	
    for (var i = 0; i < 16; i++)
    {
      if (dirImg.files && dirImg.files[i])
      {
		 numImg = i.toString().padStart(2, '0');
         let divImg = document.getElementById("img" + numImg);
         divImg.src = URL.createObjectURL(dirImg.files[i]);
	 	 divImg.style.visibility = "visible";
      }
    }
}

function selectImg(objImg)
{	
	const API_URL = window.location.protocol + "//" + window.location.host + "/api/prediction";
	
    const dirImg = document.getElementById("dirImages");
    
	i = parseInt(objImg.id.substring(3))
    
	const canvas = document.getElementById("imgCanvas");
    const ctx = canvas.getContext("2d");
	
	tmpImg = new Image();
    tmpImg.src = URL.createObjectURL(dirImg.files[i]);
	tmpImg.onload = function() {
       ctx.drawImage(tmpImg, 0, 0, 480, 480, 0, 0, 480, 480);
	}
	
    const formData = new FormData();
    formData.append("fileImg", dirImg.files[i]);
    
    let requestOpt = {}; 
    requestOpt.method = "POST";
    requestOpt.body = formData;
    
    //event.preventDefault();
     
    fetch(`${API_URL}`, requestOpt).then(res => res.json()).catch(error => console.error('Error:', error)).then(response => {
        const result = response.result;
        		
		if (result.substring(0, 5) != "ERROR")
		{
			const lblResult = document.getElementById("lblResult");
			const coord0 = response.coord0;
            const coord1 = response.coord1;
            const coord2 = response.coord2;
            const coord3 = response.coord3;
			
			lblResult.innerHTML = "This is..." + result;
			
			// draw a rectangle
			ctx.strokeStyle = "#00FF00"
			ctx.lineWidth = 7;
			ctx.strokeRect(coord0, coord1, (coord2 - coord0), (coord3 - coord1));
		}
		else
		{
			alert (result);
		}
   });
}
