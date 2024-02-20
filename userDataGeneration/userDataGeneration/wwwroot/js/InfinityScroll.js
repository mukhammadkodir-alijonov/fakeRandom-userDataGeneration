function InfinityScroll(iTable, iTbody, iAction, iParams) {
    this.table = iTable; // Reference to the table where data should be added
    this.tbody = iTbody;
    this.action = iAction; // Name of the conrtoller action
    this.params = iParams; // Additional parameters to pass to the controller
    this.loading = false; // true if asynchronous loading is in process
    this.page = 1;
    this.locale = "ru";
    this.errors = 0;
    this.seed = 0;
    this.refresh = false;
    this.AddTableLines = function (itemsCount) {
        this.loading = true;
        this.params.page = self.page;
        this.params.locale = self.locale;
        this.params.errors = self.errors;
        this.params.seed = self.seed;
        this.params.itemsCount = itemsCount;
        this.params.refresh = self.refresh;
        // $("#footer").css("display", "block"); // show loading info
        $.ajax({
            type: 'POST',
            url: self.action,
            data: self.params,
            dataType: "html"
        })
            .done(function (result) {
                if (result) {
                    if (self.refresh) {
                        $('#' + self.table + ' tbody').empty();
                        $('#' + self.table + ' tbody').html(result);
                        $('#' + self.table + ' tr').not('thead tr, tbody tr').remove();
                        self.loading = false;
                        self.refresh = false;
                    }
                    else {
                        $('#' + self.table + ' tbody').append(result);
                        self.loading = false;
                    }
                }
            })
            .fail(function (xhr, ajaxOptions, thrownError) {
                console.log("Error in AddTableLines:", thrownError);
            })
            .always(function () {
                // $("#footer").css("display", "none"); // hide loading info
            });
    };

    var self = this;
    window.onscroll = function (ev) {
        if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
            //User is currently at the bottom of the page
            if (!self.loading) {
                var itemCount = $('#' + self.table + ' tr').length - 1;
                self.page += 1;
                self.AddTableLines(itemCount);

            }
        }
    };

    $(document).ready(function () {
        const $language = $('#language-select');
        const $slider = $('#slider');
        const $sliderValue = $('#slider-value');
        const $errorField = $('#error-field');
        const $seedField = $('#seed-field');
        const $randomButton = $('#seed-random-button');
        // Обработчик изменений ползунка
        $slider.on('input', function () {
            $sliderValue.text(this.value);
            $errorField.val(this.value);
        });
        // Обработчик изменений текстового поля
        $errorField.on('input', function () {
            $slider.val(this.value);
            $sliderValue.text(this.value);
            $errorField.triggerHandler('change')

        });
        // Обработчик кнопки рандома
        $randomButton.on('click', function () {
            const randomValue = Math.floor(Math.random() * (8675309 - 0) + 0);
            $seedField.val(randomValue);
            $seedField.trigger('change');
        });

        $language.change(function () {
            self.locale = $language.val().toString();
            self.page = 1;
            self.refresh = true;
            self.AddTableLines(1);
        });
        $slider.change(function () {
            self.errors = $slider.val();
            self.page = 1;
            self.refresh = true;
            self.AddTableLines(1);
        });
        $errorField.change(function () {
            var errors = Number($errorField.val());
            if (errors <= 1000) {
                self.errors = errors;
                self.page = 1;
                self.refresh = true;
                self.AddTableLines(1);
            } else {
                $errorField.val("Incorrect value")
            }



        });
        $seedField.change(function () {
            self.seed = $seedField.val();
            self.page = 1;
            self.refresh = true;
            self.AddTableLines(1);
        });
    });
    this.AddTableLines(1);
}