<template>
    <div class="col-md-6 admincontent">
        <div class="form-group admin_content_part">
            <div class="col-md-12">
                <label class="control-label">標題</label>
                <input v-model="edit.Title" class="form-control" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-12">
                <label class="control-label">內容</label>
                <div>
                    <vue-ckeditor v-model="edit.Content"></vue-ckeditor>
                </div>
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6">
                <label class="control-label">上檔日期</label>
                <input v-model="edit.PublishDate" class="form-control datestart" />
                <input type="hidden" class="hidPublishDate" />
            </div>
            <div class="col-md-6">
                <label class="control-label">下檔日期</label>
                <input v-model="edit.DownDate" class="form-control dateend" />
                <input type="hidden" class="hidDownDate" />
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6 no_chksend">
                <input type="submit" value="儲存" class="btn btn-info" v-on:click='editAbout' />
            </div>
        </div>
    </div>
</template>

<script>
    module.exports = {
        data: function () {
            return {
                fileInfo:
                {
                    file: null,
                    imageData: ''
                }
                ,
                edit: {
                    ID: null,
                    Title: null,
                    Content: null,
                    PublishDate: null,
                    DownDate: null
                }
            }
        },
        mounted: function () {
            this.getAbout();
            $(".dateend").datepicker().on("change", function (e) {
                $(".hidDownDate").val($(this).val());
            });
            $(".datestart").datepicker().on("change", function (e) {
                $(".hidPublishDate").val($(this).val());
            });
        },
        methods: {
            editAbout: function () {
                var self = this;
                self.edit.PublishDate = $(".hidPublishDate").val();
                self.edit.DownDate = $(".hidDownDate").val();

                $.ajax({
                    type: 'Put',
                    url: '/api/admin/editabout',
                    data: self.edit,
                    statusCode: {},
                    success: function () {
                        alert("新增成功");
                        location.href = "/Abouts/Index";
                    },
                    error: function (xhr, textStatus, error) {
                        alert(xhr.responseJSON.Message);
                    }
                });
            },
            getAbout: function () {
                var self = this;
                var url = window.location.search;
                var arr = url.split("id=");
                if (arr.length > 1)
                    var id = arr[1];

                $.get("/api/admin/getabout?id=" + id)
                    .done(function (result) {
                        self.edit.Title = result.title;
                        self.edit.Content = result.content;
                        self.edit.DownDate = result.downDate;
                        self.edit.PublishDate = result.publishDate;
                        self.edit.ID = result.id;
                        $(".hidPublishDate").val(result.publishDate);
                        $(".hidDownDate").val(result.downDate);
                    })
                    .fail(function (xhr) {
                        alert(xhr.responseJSON.Message);
                    });
            }
        },
        components: {
            'vue-ckeditor': httpVueLoader('/vue/VueCkeditor.vue')
        }
    }
</script>
