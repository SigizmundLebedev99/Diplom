<template>
    <div>
        <v-container v-if="loading && number">
            <v-layout column justify-center align-center fill-height>
                <v-progress-circular indeterminate color="primary"></v-progress-circular>
            </v-layout>
        </v-container>
        <div v-else>
            <v-layout row justify-end wrap fill-height>
                <v-flex md8 xs12>
                    <v-toolbar dark :class="currentWiType.color" dense flat>
                        <v-toolbar-title>
                            {{currentWiType.name}} - {{currentWI.changed.number}}
                        </v-toolbar-title>
                    </v-toolbar>
                    <v-container class="pt-0 divide">
                        <v-layout column>
                            <v-subheader class="pl-0 mt-1 subtitle">Название</v-subheader>
                            <v-text-field v-model="currentWI.changed.name" class="pt-0 mt-0">
                            </v-text-field>
                            <v-subheader class="pl-0 subtitle">Описание</v-subheader>
                            <ckeditor :editor="editor" v-model="currentWI.changed.description.description" :config="editorConfig"></ckeditor>
                            <v-layout row wrap>
                                <v-flex xs12 sm6 v-if="currentWiType.parent">
                                    <v-layout align-center>
                                        <v-subheader class="pl-0">Предок</v-subheader>
                                        <wi-selector :code="currentWiType.parent" @selected="parentSelected" icon="edit"/>
                                        <v-btn small icon class="ml-0" @click="dropParent()">
                                            <v-icon small>close</v-icon>
                                        </v-btn> 
                                    </v-layout>
                                    <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                                    <router-link v-if="currentWI.changed.parent" :to="{name:currentWI.changed.parent.code, params:{number:currentWI.changed.parent.number}}">
                                        <span>{{`${currentWI.changed.parent.code}${currentWI.changed.parent.number} - ${currentWI.changed.parent.name}`}}</span>
                                    </router-link><span v-else>Предок не выбран</span>
                                </v-flex>
                                <v-flex xs12 sm6 v-if="currentWiType.epickLink">
                                    <v-layout align-center>
                                        <v-subheader class="pl-0">Epick link</v-subheader>
                                        <wi-selector code="EPICK" @selected="epickSelected" icon="edit"/>
                                        <v-btn small icon class="ml-0" @click="dropEpick()">
                                            <v-icon small>close</v-icon>
                                        </v-btn> 
                                    </v-layout>
                                    <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                                    <router-link v-if="currentWI.changed.epick" :to="{name:currentWI.changed.epick.code, params:{number:currentWI.changed.epick.number}}">
                                        <span>{{`${currentWI.changed.epick.code}${currentWI.changed.epick.number} - ${currentWI.changed.epick.name}`}}</span>
                                    </router-link>
                                    <span v-else>Epick не выбран</span>
                                </v-flex>
                            </v-layout>
                        </v-layout>  
                    </v-container>       
                </v-flex>
                <v-flex md4 xs12>
                    <v-tabs dark
                        :color="currentWiType.color"
                        v-model="model"
                        centered
                        slider-color="yellow">
                        <v-tab ripple>
                            Комментарии
                        </v-tab>
                        <v-tab ripple>
                            Вложения
                        </v-tab>
                        <v-tab ripple>
                            История
                        </v-tab>
                    </v-tabs>
                    <v-tabs-items v-model="model">
                        <v-tab-item>
                            <v-card flat>
                            <v-card-text>Комментарии</v-card-text>
                            </v-card>
                        </v-tab-item>
                        <v-tab-item>
                            <files></files>
                        </v-tab-item>
                        <v-tab-item>
                            <v-card flat>
                            <v-card-text>История</v-card-text>
                            </v-card>
                        </v-tab-item>
                    </v-tabs-items>
                </v-flex>
            </v-layout>
        </div>
    </div>
</template>

<script>
import {mapGetters, mapMutations} from 'vuex'
import onResize from '../../mixins/on-resize'
import ClassicEditor from '@ckeditor/ckeditor5-build-classic'
import adapters from '../../image-upload-adapter'
import files from './files'
import workItems from '../../data/work-items-object'
import wiSelector from './wi-selector'

export default {
    mixins:[onResize],
    components:{
        'files':files,
        'wi-selector': wiSelector
    },
    mounted(){
        this.enter();
    },
    data:()=>({
        loading:true,
        model:null,
        editor: ClassicEditor,
        editorConfig: {
            extraPlugins: [ adapters.updateWIAdapter ]
        }
    }),
    computed:{
        number(){
            return this.$route.params.number;
        },
        currentWiType(){
            return workItems[this.$route.name];
        },
        ...mapGetters({currentWI:'project/currentWI'})
    },
    methods:{
        enter(){
            if(this.currentWI){
                this.loading=false;
            }
            else{
                this.loading = true;
                this.$http.get(`/api/workitems/project/${this.$route.params.projId}/item/${this.$route.name}/${this.number}`)
                .then(
                    r=>{
                        r.data.changed = Object.assign({}, r.data);
                        this.addWI(r.data);
                        this.loading = false;
                    },
                    r=>console.log(r.response)
                );
            }
        },
        parentSelected(item){
            this.currentWI.changed.parent = item;
        },
        dropParent(){
            this.currentWI.changed.parent = null;
        },
        epickSelected(item){
            this.currentWI.changed.epick = item;
        },
        dropEpick(){
            this.currentWI.changed.epick = null;
        },
        ...mapMutations({addWI:'project/addWI'})
    },
    watch:{
        number(from, to){
            if(!to)
                return;
            this.enter();
        }
    }
}
</script>

<style scoped>
.divide{
    border-right: 1px solid gray;
}
.subtitle{
    height: 30px;
}
</style>


