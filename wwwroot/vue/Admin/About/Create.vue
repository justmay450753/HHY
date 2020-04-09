<template>
    <div class="col-md-6 admincontent">
        <div class="form-group admin_content_part">
            <div class="col-md-12">
                <label class="control-label">標題</label>
                <input v-model="insert.Title" class="form-control" />
            </div>
        </div>
        <div class="form-group">
            <div class="col-md-12">
                <label class="control-label">內容</label>
                <div>
                    <vue-ckeditor v-model="insert.Content"></vue-ckeditor>
                </div>
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6">
                <label class="control-label">上檔日期</label>
                <input v-model="insert.PublishDate" class="form-control datestart" />
                <input type="hidden" class="hidPublishDate" />
            </div>
            <div class="col-md-6">
                <label class="control-label">下檔日期</label>
                <input v-model="insert.DownDate" class="form-control dateend" />
                <input type="hidden" class="hidDownDate" />
            </div>
        </div>
        <div class="form-group admin_content_part">
            <div class="col-md-6 no_chksend">
                <input type="submit" value="建立" class="btn btn-info" v-on:click='createAbout' />
            </div>
        </div>
    </div>
</template>

<script>
    module.exports = {
        data: function () {
            return {
                insert: {
                    Title: null,
                    Content: null,
                    PublishDate: null,
                    DownDate: null,
                    IsPublish: null,
                }
            }
        },
        mounted: function () {
            $(".dateend").datepicker().on("change", function (e) {
                $(".hidDownDate").val($(this).val());
            });
            $(".datestart").datepicker().on("change", function (e) {
                $(".hidPublishDate").val($(this).val());
            });
        },
        methods: {
            createAbout: function () {
                var self = this;
                self.insert.PublishDate = $(".PublishDate").val();
                self.insert.DownDate = $(".hidDownDate").val();
                $.post("/api/admin/createabout", self.insert)
                    .done(function () {
                        alert("新增成功");
                        location.href = "/Abouts/Index";
                    })
                    .fail(function (xhr, textStatus, error) {
                        alert(xhr.responseJSON.Message);
                    })
            }
        },
        components: {
            'vue-ckeditor': httpVueLoader('/vue/VueCkeditor.vue')
        }
    }
</script>
