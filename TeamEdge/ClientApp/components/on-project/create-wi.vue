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
                <v-tabs
                    v-model="tabModel"
                    centered
                    slider-color="black">
                    <v-tab class="text-none">
                        Описание
                    </v-tab>
                    <v-tab class="text-none">
                        Вложения
                    </v-tab>
                    <v-tab class="text-none">
                        Дополнительно
                    </v-tab>
                </v-tabs>
                <v-tabs-items v-model="tabModel">
                    <v-tab-item>
                        <v-form ref="form" v-model="valid">
                            <v-subheader>Тип единицы работы</v-subheader>
                            <v-select class="ml-3 pt-0 mt-0 mr-3"
                                v-model="wiType"
                                :items="workItems"
                                :rules="rules.typeRule"
                                @change="clearAdditional"
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
                    </v-tab-item>
                    <v-tab-item>
                        <div class="block">
                            <v-layout row class="mt-1">
                                <v-subheader>Прикрепленные файлы</v-subheader>
                                <v-spacer></v-spacer>
                                <v-btn icon @click="openFiles()">
                                    <v-icon>
                                        attach_file
                                    </v-icon>
                                </v-btn>
                            </v-layout>
                            <v-layout row wrap justify-center>
                                <v-card v-for="(f,i) in selectedFiles" :key="i" class="mx-1 my-1">
                                    <v-card-text class="mx-1 my-1">
                                        <img height="128px" v-if="f.isPicture" :src="f.path"/>
                                        <v-icon v-else>
                                            insert_drive_file
                                        </v-icon>
                                    </v-card-text>
                                </v-card>
                            </v-layout>
                        </div>
                    </v-tab-item>
                    <v-tab-item>
                        <additional-info :itemType="wiType" ref="additional"></additional-info>
                    </v-tab-item>
                </v-tabs-items>
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
import adapters from '../../image-upload-adapter'
import workItems from '../../data/work-items-array'
import formValidation from '../../mixins/form-validation'
import additionalInfo from './additional-info'
export default {
    components:{
        'additional-info':additionalInfo
    },
    mixins:[onResize, formValidation],
    data:()=>({
        model:{
            name:'',
            descriptionText:''
        },
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ adapters.createWIAdapter ]
        },
        wiType:null,
        rules:{
            nameRule:[v=>!!v||'Введите имя'],
            typeRule:[v=>!!v||'Выберите тип']
        },
        valid:true,
        loading:false,
        tabModel:null
    }),
    methods:{
        ...mapActions({
            setDialog:'createWorkItem/setDialog', 
            openFiles:'fileSelector/open'
        }),
        ...mapMutations({
            addWI:'project/addWI',
        }),
        submit(){
            this.validate()
            if(!this.valid)
                return;
            this.loading = true;
            var model = {
                code:this.wiType.code,
                projectId:this.project.id,
                fileIds: this.selectedFiles.map(e => e.id),
                ...this.model,
                ...this.additionalInfo
            };
            this.$http.post(`/api/workitems`, model)
            .then(
              r => {
                  r.data.changed = Object.assign({}, r.data);
                  this.addWI(r.data);
                  this.$router.push({ name: r.data.code, params: { number: r.data.number } })
                  this.loading = false;
                  this.close();
              },
              r => {console.log('error', r.response); this.loading = false;}
            );
        },
        close(){
            this.setDialog(false);
            this.resetValidation();
            this.reset();
            this.model.descriptionText = '';
            this.clearAdditional();
        },
        clearAdditional(){
            this.$refs.additional.clear();
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
            project: 'project/project',
            additionalInfo: 'createWorkItem/additionalInfo',
            predefined: 'createWorkItem/predefined'
        })
    },
    watch:{
        dialog(to,from){
            if(!to)
                return;
            if(this.predefined && this.predefined.parentCode){
                this.wiType = workItems.find(e=>!e.parent?false:
                e.parent.startsWith(this.predefined.parentCode));
            }
        }
    }
}
</script>

<style scoped>
    .block{
        min-height: 300px;
    }
</style>


