function initializeEmojiPicker(buttonId, inputId) {
    document.addEventListener('DOMContentLoaded', function () {
        const button = document.getElementById(buttonId);
        const input = document.getElementById(inputId);
        const pickerOptions = {
            onEmojiSelect: emoji => {

                input.value += emoji.native;
                input.focus();
                hidePicker(); // Hidin picker after selecting an emoji. Might change later
            },
            set: "google"
        };
        const picker = new EmojiMart.Picker(pickerOptions);

        // Trying to keep picker near button
        function positionPicker() {
            const buttonRect = button.getBoundingClientRect();
            picker.style.position = 'absolute';
            picker.style.left = `${buttonRect.left + window.scrollX}px`;
            picker.style.top = `${buttonRect.bottom + window.scrollY}px`;
        }

        // showing picker
        function showPicker() {
            positionPicker();
            picker.style.display = '';
            document.addEventListener('click', handleClickOutside, false);
        }

        // hiding picker
        function hidePicker() {
            picker.style.display = 'none';
            document.removeEventListener('click', handleClickOutside, false);
        }

        // Outside click should close picker
        function handleClickOutside(event) {
            if (!picker.contains(event.target) && event.target !== button) {
                hidePicker();
            }
        }

        // Hide picker by default and toggle it on button click
        hidePicker();
        document.body.appendChild(picker);

        button.addEventListener('click', () => {
            if (picker.style.display === 'none') {
                showPicker();
            } else {
                hidePicker();
            }
        });
    });
}
