
document.addEventListener("DOMContentLoaded", function () {
    const burger = document.getElementById("burgerBtn");
    const nav = document.getElementById("mobileNav");

    burger.addEventListener("click", () => {
        nav.classList.toggle("active");
    });
    
    document.querySelectorAll(".submenu-toggle").forEach(btn => {
        btn.addEventListener("click", () => {
            const parent = btn.closest(".has-submenu");
            parent.classList.toggle("active");
        });
    });
});


function toggleIdea(id) {
    const content = document.getElementById('idea-content-' + id);
    const arrow = document.getElementById('idea-arrow-' + id);

    content.classList.toggle('open');
    arrow.textContent = content.classList.contains('open') ? '▲' : '▼';
}
