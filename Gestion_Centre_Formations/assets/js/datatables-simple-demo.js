window.addEventListener('DOMContentLoaded', event => {
    // Initialize Simple-DataTables
    // https://github.com/fiduswriter/Simple-DataTables/wiki

    const datatablesSimple = document.getElementById('tablesSimple'); 
    // Initialize the first table
    if (datatablesSimple) {
        new simpleDatatables.DataTable(datatablesSimple, {
            paging: false,
        });
    }
});
