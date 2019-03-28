<template>
    <v-dialog v-model="dialog" :width="ofSize({xs:'95%', sm:'70%', md:'50%'})" scrollable persistent>
        <v-card>
            <v-card-title class="primary white--text pt-0 pb-0 pr-0">
                <v-layout justify-space-between align-center>
                    <span class="title">Создание единицы работы</span>
                    <v-btn icon dark @click="close()">
                        <v-icon>
                            close
                        </v-icon>
                    </v-btn>
                </v-layout>
            </v-card-title>
            <v-card-text class="pt-0">
                <v-form ref="form" v-model="valid">
                    <v-subheader>Тип единицы работы</v-subheader>
                    <v-select class="ml-3 pt-0 mt-0 mr-3"
                        v-model="wiType"
                        :items="workItems"
                        :rules="rules.typeRule"
                        required>
                        <template v-slot:selection="data">
                            <span>{{data.item.name}}</span>
                        </template>
                        <template v-slot:item="{ index, item }">
                            <span>{{item.name}}</span>
                        </template>
                    </v-select>
                    <v-subheader>Название</v-subheader>
                    <v-text-field required :rules="rules.nameRule" v-model="model.name" class="ml-3 pt-0 mt-0 mr-3"></v-text-field>
                    <v-subheader>Описание</v-subheader>
                    <div class="ml-3 mr-3">
                        <ckeditor :editor="editor" v-model="model.descriptionText" :config="editorConfig"></ckeditor>
                    </div>
                </v-form>
                <v-layout row class="mt-1">
                    <v-subheader>Прикрепленные файлы</v-subheader>
                    <v-spacer></v-spacer>
                    <v-btn icon @click="openFiles()">
                        <v-icon>
                            attach_file
                        </v-icon>
                    </v-btn>
                </v-layout>
                <v-layout row wrap class="mr-3 ml-3 mb-1 mt-1">
                    <v-chip label v-for="(f,i) in selectedFiles" :key="i" class="primary" dark @click="addFile(f)">
                        <div class="mr-1 ml-1">
                            <v-icon v-if="f.isPicture">
                                image
                            </v-icon>
                            <v-icon v-else>
                                insert_drive_file
                            </v-icon>
                        </div>
                        <span>{{f.fileName}}</span>   
                    </v-chip>
                </v-layout>

            </v-card-text>
            <v-card-actions>
                
                <v-spacer></v-spacer>
                <v-btn class="primary" @click="submit()" :loading="loading">Создать</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script>
import {mapGetters, mapActions, mapMutations} from 'vuex'
import onResize from '../../mixins/on-resize'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import MyCustomUploadAdapterPlugin from '../../image-upload-adapter'
import workItems from '../../data/work-items'
import formValidation from '../../mixins/form-validation'
export default {
    mixins:[onResize, formValidation],
    data:()=>({
        model:{
            name:'',
            descriptionText:''
        },
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ MyCustomUploadAdapterPlugin ]
        },
        wiType:null,
        rules:{
            nameRule:[v=>!!v||'Введите имя'],
            typeRule:[v=>!!v||'Выберите тип']
        },
        valid:true,
        loading:false
    }),
    methods:{
        ...mapActions({setDialog:'createWorkItem/setDialog', openFiles:'fileSelector/open'}),
        ...mapMutations({
            addWI:'project/addWI'
        }),
        submit(){
            this.validate()
            if(!this.valid)
                return;
            this.loading = true;
            var model = Object.assign({
                code:this.wiType.code,
                projectId:this.project.id,
                fileIds: this.selectedFiles.map(e=>e.id), 
            }, this.model);
            console.log(model);
            this.$http.post(`/api/workitems`, model)
            .then(
                r=>{
                    this.addWI(r.data); 
                    this.$router.push({name:r.data.code, params:{number:r.data.number}})
                    this.loading = false;
                    this.close();
                },
                r=>console.log('error', r.response)
            )
        },
        close(){
            this.setDialog(false);
            this.resetValidation();
            this.reset();
        }
    },
    computed:{
        dialog:{
            ...mapGetters({get:'createWorkItem/dialog'}),
            set(value){
                this.setDialog(value);
            }
        },
        workItems(){
            return workItems;
        },
        ...mapGetters({
            selectedFiles:'fileSelector/selectedFiles',
            project:'project/project'
        })
    }
}
</script>

