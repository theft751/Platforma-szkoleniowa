function DeleteFilm(id)
{
    let doYouWantToDelete = confirm("Do you realy want to delete this film?");
    if (doYouWantToDelete) {
        fetch(`/Films/Delete/${id}`, { method: 'DELETE' })
            .then((res) => {
                if (res.ok) {
                    location.reload();
                    alert("Film deleted succesfully!");

                }
                else { alert("Film removing failed"); }
            })
    }
}