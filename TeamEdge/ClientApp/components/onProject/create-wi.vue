<template>
    <v-dialog v-model="dialog" :width="ofSize({xs:'95%', sm:'70%', md:'50%'})" scrollable persistent>
        <v-card>
            <v-card-title class="primary white--text pt-0 pb-0 pr-0">
                <v-layout justify-space-between align-center>
                    <span class="title">Создание единицы работы</span>
                    <v-btn icon dark @click="setDialog(false)">
                        <v-icon>
                            close
                        </v-icon>
                    </v-btn>
                </v-layout>
            </v-card-title>
            <v-card-text>
                <v-form>
                    <v-subheader>Тип единицы работы</v-subheader>
                    <v-select class="ml-3 pt-0 mt-0 mr-3"
                        v-model="wiType"
                        :items="workItems">
                        <template v-slot:selection="data">
                            <span>{{data.item.name}}</span>
                        </template>
                        <template v-slot:item="{ index, item }">
                            <span>{{item.name}}</span>
                        </template>
                    </v-select>
                    <v-subheader>Название</v-subheader>
                    <v-text-field v-model="model.name" class="ml-3 pt-0 mt-0 mr-3"></v-text-field>
                    <v-subheader>Описание</v-subheader>
                    <div class="ml-3 mr-3">
                        <ckeditor :editor="editor" v-model="model.descriptionText" :config="editorConfig"></ckeditor>
                    </div>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-btn icon @click="openFiles()">
                    <v-icon>
                        attach_file
                    </v-icon>
                </v-btn>
                <v-spacer></v-spacer>
                <v-btn class="primary" @click="submit()">Создать</v-btn>
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
export default {
    mixins:[onResize],
    data:()=>({
        model:{
            name:null,
            descriptionText:''
        },
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ MyCustomUploadAdapterPlugin ]
        },
        wiType:null
    }),
    methods:{
        ...mapActions({setDialog:'createWorkItem/setDialog', openFiles:'fileSelector/open'}),
        submit(){
            
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
        }
    }
}
</script>

