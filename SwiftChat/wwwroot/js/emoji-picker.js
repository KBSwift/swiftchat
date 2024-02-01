function initializeEmojiPicker(buttonId, inputId) {
    document.addEventListener('DOMContentLoaded', function () {
        const button = document.getElementById(buttonId);
        const input = document.getElementById(inputId);
        let savedRange = null;
        let isInputBlurred = true;
        const pickerOptions = {
            onEmojiSelect: emoji => {
                insertEmojiAtCursor(input, emoji.native);
                input.focus();
                hidePicker(); // Hiding picker after selecting an emoji. Might change later
            },
            set: "native"
        };
        const picker = new EmojiMart.Picker(pickerOptions);
        
        function saveSelection() {
            const sel = window.getSelection();
            if (sel.rangeCount > 0) {
                savedRange = sel.getRangeAt(0);
            }
        }
        function restoreSelection() {
            const sel = window.getSelection();
            if (savedRange && input.getAttribute("is-data-placeholder") === "false") {
                sel.removeAllRanges();
                sel.addRange(savedRange);

            }
        }

        input.addEventListener("blur", (event) => {
            if (event.relatedTarget !== button) {
                isInputBlurred = true;
            }
            saveSelection;
            //console.log("addEventBlurValue", isInputBlurred);
        });
        input.addEventListener("focus", () => {
            isInputBlurred = false;
            restoreSelection;
            //console.log("addEventFocusValue", isInputBlurred);
        });

        function insertCaretAtLineEnd() {
            const sel = window.getSelection();
            const range = document.createRange();
            range.selectNodeContents(input);
            range.collapse(false);
            sel.removeAllRanges();
            sel.addRange(range);
        }
        function insertEmojiAtCursor(input, emoji) {
            restoreSelection();
            const sel = window.getSelection();
            if (!isInputBlurred && input.getAttribute("is-data-placeholder") === "false") {
                const range = sel.getRangeAt(0);
                range.deleteContents();
                const emojiNode = document.createTextNode(emoji);
                range.insertNode(emojiNode);

                // Moving caret immediately after this inserted emoji
                range.setStartAfter(emojiNode);
                range.setEndAfter(emojiNode);
                sel.removeAllRanges();
                sel.addRange(range);
            } else if (isInputBlurred && input.textContent && input.getAttribute("is-data-placeholder") === "false") {
                input.innerHTML += emoji;
                insertCaretAtLineEnd();
            } else {
                input.innerHTML = "";
                input.innerHTML += emoji;
                insertCaretAtLineEnd();
            }
        }

        // Picker positioning and view logic
        function positionPicker() {
            const buttonRect = button.getBoundingClientRect();
            picker.style.position = 'absolute';
            picker.style.left = `${buttonRect.left + window.scrollX}px`;
            picker.style.top = `${buttonRect.bottom + window.scrollY}px`;
        }
        
        function showPicker() {
            positionPicker();
            picker.style.display = '';
            document.addEventListener('click', handleClickOutside, false);
        }
        
        function hidePicker() {
            picker.style.display = 'none';
            document.removeEventListener('click', handleClickOutside, false);
        }
        
        function handleClickOutside(event) {
            if (!picker.contains(event.target) && event.target !== button) {
                hidePicker();
            }
        }
        
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
