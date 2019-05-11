<template>
    <v-container>
        <p v-if="!itemType">Выберите тип единицы работы</p>
        <div v-else>
            <div v-if="itemType.parent">
                <v-layout align-center>
                    <v-subheader class="pl-0">Предок</v-subheader>
                    <wi-selector :code="itemType.parent" @selected="parentSelected" icon="edit"></wi-selector>
                    <v-btn small icon class="ml-0" @click="dropParent()" v-if="!itemType.requireParent">
                        <v-icon small>close</v-icon>
                    </v-btn> 
                    <sub v-if="itemType.requireParent">Необходимо выбрать предка</sub>
                </v-layout>
                <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                <span v-if="currentParent">{{`${currentParent.code}${currentParent.number} - ${currentParent.name}`}}</span>
                <span v-else>Предок не выбран</span>
            </div>
            <div v-if="itemType.epickLink && !parent" class="mt-3">
                <v-layout align-center>
                    <v-subheader class="pl-0">Epick link</v-subheader>
                    <wi-selector code="EPICK" @selected="epickSelected" icon="edit"></wi-selector>
                    <v-btn small icon class="ml-0" @click="dropEpickLink()">
                        <v-icon small>close</v-icon>
                    </v-btn>
                </v-layout>
                <v-divider class="mr-3 mt-0 mb-2"></v-divider>
                <span v-if="epick">{{`${epick.code}${epick.number} - ${epick.name}`}}</span>
                <span v-else>Epick не выбран</span>
            </div>
            <div v-if="itemType.children" class="mt-3">
              <v-layout align-center>
                <v-subheader class="pl-0">Потомки</v-subheader>
                <wi-selector :code="itemType.children" @selected="addChild"></wi-selector>
              </v-layout>
              <v-divider class="mr-3 mt-0 mb-2"></v-divider>
              <span v-if="!children.length">Не выбран ни один потомок</span>
              <v-list dense>
                <v-list-tile v-for="(item, i) in children" :key="i">
                  <v-layout row justify-space-between align-center>
                    <span>{{`${i+1}) ${item.code}${item.number} - ${item.name}`}}</span>
                    <v-btn small icon @click="removeChild(item.descriptionId)">
                      <v-icon small>close</v-icon>
                    </v-btn>
                  </v-layout>
                </v-list-tile>
              </v-list>
            </div>
        </div>
    </v-container>
</template>

<script>
import {mapMutations, mapGetters} from 'vuex'
import wiSelector from './wi-selector'
export default {
    components:{
        'wi-selector': wiSelector
    },
    props:[
        'itemType'
    ],
    data:()=>({
        parent:null,
        children:[],
        epick:null,
    }),
    methods:{
        ...mapMutations({
            setAdditionalInfo:'createWorkItem/setAdditionalInfo'
        }),
        parentSelected(item){
            this.additionalInfo.parentId = item.descriptionId;
            this.parent = item;
        },
        dropParent(){
            this.additionalInfo.parentId = null;
            this.parent = null;
        },
        epickSelected(item){
            this.epick = item;
            this.additionalInfo.epickId = item.descriptionId;
        },
        dropEpickLink(){
            this.epick = null;
            this.additionalInfo.epickId = null;
        },
        addChild(item){
            if(!this.additionalInfo.childrenIds)
                this.additionalInfo.childrenIds = [];
            var arr = [];
            if(this.additionalInfo.childrenIds.some(id=>id==item.descriptionId))
                return;
            if(this.itemType.code == 'EPICK' && item.code != 'STORY'){       
                if(!this.additionalInfo.linkIds)
                    this.additionalInfo.linkIds = [];
                if(this.additionalInfo.linkIds.some(id=>id==item.descriptionId))
                    return;
                this.additionalInfo.linkIds.push(item.descriptionId)              
            }
            else
                this.additionalInfo.childrenIds.push(item.descriptionId);
            this.children.push(item);
        },
        removeChild(id){
            this.children = this.children.filter(e=>e.descriptionId != id);
            this.additionalInfo.childrenIds = this.children.map(e=>e.descriptionId);
            if(this.itemType.code == 'EPICK')
                this.additionalInfo.linkIds = this.additionalInfo.linkIds.filter(e=>e!=id);
        },
        clear(){
            this.setAdditionalInfo({});
            this.parent = null,
            this.epick = null,
            this.children = []
        }
    },
    computed:{
        ...mapGetters({
            additionalInfo:'createWorkItem/additionalInfo',
            predefined:'createWorkItem/predefined'
        }),
        currentParent(){
            return this.parent || this.predefined.parent;
        }
    },
    watch:{
        additionalInfo(){
            if(this.additionalInfo.parent){
                this.parent = this.additionalInfo.parent;
                this.additionalInfo.parentId = this.parent.descriptionId
            }
        }
    }
}
</script>
