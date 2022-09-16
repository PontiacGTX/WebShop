// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelector("#navBar").addEventListener("mouseover", toggleNavBarClass);



function toggleNavBarClass(element)
{
  let ele=  document.getElementById(element.target.id);
  ele =element.srcElement;
  if(ele.classList==null)
          ele.classList.add('nav-extended');
else if(!ele.classList.contains('nav-extended'))
    ele.classList.add('nav-extended');
}
