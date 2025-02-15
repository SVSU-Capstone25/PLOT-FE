<script>
    function updateInputStyle(elementId, borderColor, textColor) {
        var input = document.getElementById(elementId);
        if (input) {
            input.style.borderColor = borderColor;
            input.style.color = textColor;
        }
    }
</script>