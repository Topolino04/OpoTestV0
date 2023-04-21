window.CKEditorInterop = (() => {
    const editors = {};
    return {
        init(id, dotNetReference) {
            ClassicEditor
                .create(document.getElementById(id), {
					toolbar: {
						items: [
							'heading',
							'fontSize',
							'fontFamily',
							'|',
							'bold',
							'italic',
							'underline',
							'fontColor',
							'highlight',
							'fontBackgroundColor',
							'|',
							'link',
							'bulletedList',
							'numberedList',
							'|',
							'alignment',
							'indent',
							'outdent',
							'horizontalLine',
							'|',
							'blockQuote',
							'insertTable',
							'mediaEmbed',
							'|',
							'undo',
							'redo'
						]
					},
					language: 'es',
					table: {
						contentToolbar: [
							'tableColumn',
							'tableRow',
							'mergeTableCells'
						]
					},
					licenseKey: '',
                })
                .then(editor => {
                    editors[id] = editor;
                    editor.model.document.on('change:data', () => {
                        let data = editor.getData();

                        const el = document.createElement('div');
                        el.innerHTML = data;
                        if (el.innerText.trim() == '')
                            data = null;

                        dotNetReference.invokeMethodAsync('EditorDataChanged', data);
                    });
                })
                .catch(error => console.error(error));
        },
        destroy(id) {
            editors[id].destroy()
                .then(() => delete editors[id])
                .catch(error => console.log(error));
        }
    };
})();