<template>
    <div class="ckeditor">
        <textarea :name="name" :id="id" :value="value" :types="types" :config="config" :disabled="readOnlyMode">
          </textarea>
    </div>
</template>

<script>
    let inc = new Date().getTime();

    module.exports = {
        name: 'VueCkeditor',
        props: {
            name: {
                type: String,
                // default: () => `editor-${++inc}`
                default: function() {
                    return 'editor-'+ (++inc)
                } 
            },
            value: {
                type: String
            },
            id: {
                type: String,
                // default: () => `editor-${inc}`
                default: function() { 
                    return 'editor-' + inc
                }
            },
            types: {
                type: String,
                // default: () => `classic`
                default: function() {
                    return 'classic'
                } 
            },
            config: {
                type: Object,
                // default: () => { }
                default: function(){}
            },
            instanceReadyCallback: {
                type: Function
            },
            readOnlyMode: {
                type: Boolean,
                // default: () => false
                default: function(){
                    return false
                }
            }
        },
        data: function() {
            return {
                instanceValue: ''
            };
        },
        computed: {
            instance: function() {
                return CKEDITOR.instances[this.id];
            }
        },
        watch: {
            value: function(val) {
                try {
                    if (this.instance) {
                        this.update(val);
                    }
                } catch (e) { }
            },
            readOnlyMode: function(val) {
                this.instance.setReadOnly(val);
            }
        },
        mounted: function() {
            this.create();
        },
        methods: {
            create: function() {
                var self = this;
                if (typeof CKEDITOR === 'undefined') {
                    console.log('CKEDITOR is missing (http://ckeditor.com/)');
                } else {
                    if (this.types === 'inline') {
                        CKEDITOR.inline(this.id, this.config);
                    } else {
                        CKEDITOR.replace(this.id, this.config);
                    }

                    this.instance.setData(this.value);

                    this.instance.on('instanceReady', function() {
                        self.instance.setData(this.value);
                    });

                    // Ckeditor change event
                    this.instance.on('change', this.onChange);

                    // Ckeditor mode html or source
                    this.instance.on('mode', this.onMode);

                    // Ckeditor blur event
                    this.instance.on('blur', function(evt) {
                        self.$emit('blur', evt);
                    });

                    // Ckeditor focus event
                    this.instance.on('focus', function(evt) {
                        self.$emit('focus', evt);
                    });

                    // Ckeditor contentDom event
                    this.instance.on('contentDom', function(evt) {
                        self.$emit('contentDom', evt);
                    });

                    // Ckeditor dialog definition event
                    CKEDITOR.on('dialogDefinition', function(evt) {
                        self.$emit('dialogDefinition', evt);
                    });

                    // Ckeditor file upload request event
                    this.instance.on('fileUploadRequest', function(evt) {
                        self.$emit('fileUploadRequest', evt);
                    });

                    // Ckditor file upload response event
                    this.instance.on('fileUploadResponse', function(evt) {
                        setTimeout(function() {
                            self.onChange();
                        }, 0);
                        self.$emit('fileUploadResponse', evt);
                    });

                    // Listen for instanceReady event
                    if (typeof this.instanceReadyCallback !== 'undefined') {
                        this.instance.on('instanceReady', this.instanceReadyCallback);
                    }

                    // Registering the beforeDestroyed hook right after creating the instance
                    this.$once('hook:beforeDestroy', function() {
                        self.destroy();
                    });
                }
            },
            update: function(val) {
                if (this.instanceValue !== val) {
                    this.instance.setData(val, { internal: false });
                    this.instanceValue = val;
                }
            },
            destroy: function() {
                try {
                    let editor = window['CKEDITOR'];
                    if (editor.instances && editor.instances[this.id]) {
                        editor.instances[this.id].destroy();
                    }
                } catch (e) { }
            },
            onMode: function() {
                var self = this;
                if (this.instance.mode === 'source') {
                    let editable = this.instance.editable();
                    editable.attachListener(editable, 'input', function() {
                        self.onChange();
                    });
                }
            },
            onChange: function() {
                let html = this.instance.getData();
                if (html !== this.value) {
                    this.$emit('input', html);
                    this.instanceValue = html;
                }
            }
        }
    };
</script>