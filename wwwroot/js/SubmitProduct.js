document.querySelector('#submitBtn').addEventListener('click', SubmitProducts);
function SubmitProducts(element)
{
    //element.preventDefault();

    let ulContainer = document.getElementById("dropDownAndSel").getElementsByTagName('ul')[0];

    var Categories = [];
    var formData = new FormData();
    let x = document.getElementById("dropDownCategories");
    let index = 0;
    for (let i = 0; i < x.children.length; i++) {
        if (x.options[i].selected) {
           let CategoryName = x.options[i].innerHTML;
            let CategoryId = x.children[i].value;
            let Category = { CategoryId, CategoryName };
            
            if (!Categories.includes(Category)) {
                formData.append('model.Categories[' + index + '].CategoryName', CategoryName);
                formData.append('model.Categories[' + index + '].CategoryId', CategoryId);
                Categories.push(Category);
                index++;
            }
        }
    }
    //formData.append('model.Categories', JSON.stringify(Categories));
    for (var pair of formData.entries()) {
        console.log(pair[0] + ', ' + pair[1]);
    }

    console.log(formData);
    let pId = document.getElementById('pId').value;
    let ProductId = pId;
    let ProductName = document.getElementById('ProductName').value;
    if (!ProductName || ProductName.length ==0) {
        alert('ProdutName cannot be null or empty');
        return;
    }
    let priceStr = document.getElementById('productPriceId').value;
    if (!priceStr || priceStr.length == 0) {
        alert('Price cant be empty')
        return;
    }
    let Price = parseFloat(priceStr);
    
    let QuantityStr = document.getElementById('qtyNum').value;
    if (!QuantityStr || QuantityStr.length==0) {
        alert('Quantity cant be empty');
        return;
    }
    let Quantity = parseInt(QuantityStr);

    let EnabledEl = document.getElementById("checkEnabledBoxId");
    let Enabled = EnabledEl.value;
    let filesContainer = document.getElementById('photoProduct_0');
    for (let i = 0; i < filesContainer.files.length; i++) {
        formData.append('model.Photos', filesContainer.files[i], filesContainer.files[i].name);
    }

    formData.set('model.ProductId', ProductId);
    formData.set('model.Enabled', Enabled);
    formData.set('model.ProductName', ProductName);
    formData.set('model.Price', Price);
    formData.set('model.Quantity', Quantity);
    


    $.ajax({
        url: '/Product/AddProduct',
        type: 'POST',
        contentType: false,
        processData: false,
        data: formData,
        success: console.log('OK')

    });

}
