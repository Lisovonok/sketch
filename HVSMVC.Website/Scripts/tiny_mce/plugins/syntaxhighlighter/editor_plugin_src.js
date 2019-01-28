(function() {
	tinymce.create('tinymce.plugins.SyntaxHighlighter', {
		createControl: function(n, cm) {
            if (n == 'syntaxhighlighter') {    
                var mlb = cm.createListBox('mylistbox', {
                    title : 'Code Snippet',
                    onselect : function(v) {
                        if (v != '') {
                            var selection = tinyMCE.activeEditor.selection.getContent();
			                if (selection == '') {
			                    selection = 'Your ' + v + '-code';
			                }
                            tinyMCE.activeEditor.execCommand("mceInsertContent", false, '<pre class="brush: ' + v + ';">' + selection + '</pre>');
                        }
                    }
                });

                mlb.add('CSharp', 'c-sharp');
                mlb.add('Java', 'java');
                mlb.add('PHP', 'php');
                mlb.add('XML', 'xml');
                mlb.add('CSS', 'css');
                mlb.add('SQL', 'sql');

                return mlb;
            }
		},

		getInfo : function() {
			return {
				longname : 'SyntaxHighlighter',
				author : 'Daniel Palme',
				authorurl : 'http://www.Lisovonok.de',
				infourl : 'http://wiki.moxiecode.com/',
				version : 1.0
			};
		}
	});

	// Register plugin
	tinymce.PluginManager.add('syntaxhighlighter', tinymce.plugins.SyntaxHighlighter);
})();
