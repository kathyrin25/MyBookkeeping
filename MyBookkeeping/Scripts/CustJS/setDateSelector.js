function setDateTimeObject()
{
    $('[data-datetimepicker]').each(function () {
        var id = $(this).attr('id');

        $(this).pickadate({
            format: 'yyyy/mm/dd',
            selectYears: true,
            selectMonths: true,
            selectYears: 4
        });

    });
}
