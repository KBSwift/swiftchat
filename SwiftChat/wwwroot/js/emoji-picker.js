function initializeEmojiPicker(buttonId, inputId) {
    document.addEventListener('DOMContentLoaded', function () {
        const button = document.getElementById(buttonId);
        const input = document.getElementById(inputId);
        const pickerOptions = {
            onEmojiSelect: emoji => {
                if (input.getAttribute("is-data-placeholder") === "true") {
                    input.innerHTML = "";
                    input.setAttribute("is-data-placeholder", "false");
                }
                insertEmojiAtCursor(input, emoji.native);
                input.focus();
                hidePicker(); // Hiding picker after selecting an emoji. Might change later
            },
            set: "native"
        };
        const picker = new EmojiMart.Picker(pickerOptions);

        function insertEmojiAtCursor(element, emoji) {
            const sel = window.getSelection();
            if (element.getAttribute("is-data-placeholder") === "false") {
                const range = sel.getRangeAt(0);
                range.deleteContents();
                const emojiNode = document.createTextNode(emoji);
                range.insertNode(emojiNode);

                // Move the caret immediately after the inserted emoji
                range.setStartAfter(emojiNode);
                range.setEndAfter(emojiNode);
                sel.removeAllRanges();
                sel.addRange(range);
            } else if (element.textContent) {
                // Fallback: append the emoji at the end if there's no selection
                element.textContent += emoji;
            } else {
                // If the content is empty, just add the emoji
                element.textContent = emoji;
            }
        }

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
